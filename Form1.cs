using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;

namespace 汽车仓储管理系统
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            BeginListen.Enabled = true;
            CloseListen.Enabled = false;
            Park_get.Enabled = false;
            Park_out.Enabled = false;
            Park_check.Enabled = false;
        }

        Thread threadWatch = null; // 负责监听客户端连接请求的 线程；  
        Socket socketWatch = null;

        Dictionary<string, Socket> dict = new Dictionary<string, Socket>();
        Dictionary<string, Thread> dictThread = new Dictionary<string, Thread>();


        /// <summary>
        /// 按键启动服务器，开启监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtIp.Text != "" && txtPort.Text != "")
            {
                // 创建负责监听的套接字，注意其中的参数；  
                socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // 获得文本框中的IP对象；  
                IPAddress address = IPAddress.Parse(txtIp.Text.Trim());
                // 创建包含ip和端口号的网络节点对象；  
                IPEndPoint endPoint = new IPEndPoint(address, int.Parse(txtPort.Text.Trim()));
                try
                {
                    // 将负责监听的套接字绑定到唯一的ip和端口上；  
                    socketWatch.Bind(endPoint);
                }
                catch (SocketException se)
                {
                    MessageBox.Show("异常：" + se.Message);
                    return;
                }
                // 设置监听队列的长度；  
                socketWatch.Listen(10);
                // 创建负责监听的线程；  
                threadWatch = new Thread(WatchConnecting);
                threadWatch.IsBackground = true;
                threadWatch.Start();
                ShowMsg("服务器启动监听成功！");
                BeginListen.Enabled = false;
                CloseListen.Enabled = true;
                Park_get.Enabled = true;
                Park_out.Enabled = true;
                Park_check.Enabled = true;
            }
            else
            {
                ShowMsg("请输入IP和端口号！");
            }
        }

        /// <summary>
        /// 监听子函数
        /// </summary>
        void WatchConnecting()
        {
            while (true)  // 持续不断的监听客户端的连接请求；  
            {
                // 开始监听客户端连接请求，Accept方法会阻断当前的线程；  
                Socket sokConnection = socketWatch.Accept(); // 一旦监听到一个客户端的请求，就返回一个与该客户端通信的 套接字；  
                // 想列表控件中添加客户端的IP信息；  
                lbOnline.Items.Add(sokConnection.RemoteEndPoint.ToString());
                // 将与客户端连接的 套接字 对象添加到集合中；  
                dict.Add(sokConnection.RemoteEndPoint.ToString(), sokConnection);
                //txtMsg.AppendText("客户端连接，地址为：");
                ShowMsg("客户端连接，地址为：" + sokConnection.RemoteEndPoint.ToString());
                //ShowMsg(sokConnection.RemoteEndPoint.ToString());

                //接收数据线程
                Thread thr = new Thread(RecMsg);
                thr.IsBackground = true;
                thr.Start(sokConnection);
                dictThread.Add(sokConnection.RemoteEndPoint.ToString(), thr);  //  将新建的线程 添加 到线程的集合中去。  
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sokConnectionparn"></param>
        private void RecMsg(object sokConnectionparn)
        {
            string recive_data;
            Socket sokClient = sokConnectionparn as Socket;
            while (true)
            {
                // 定义一个2M的缓存区
                byte[] arrMsgRec = new byte[1024];
                // 将接受到的数据存入到输入  arrMsgRec中；  
                int length = -1;
                try
                {
                    length = sokClient.Receive(arrMsgRec); // 接收数据，并返回数据的长度； 
                    string strMsg = System.Text.Encoding.UTF8.GetString(arrMsgRec, 0, length);// 将接受到的字节数据转化成字符串
                    //txtMsg.AppendText("数据来自:" + sokClient.RemoteEndPoint.ToString() + "\r\n");
                    ShowMsg("数据来自:" + sokClient.RemoteEndPoint.ToString());
                    ShowMsg(strMsg);
                    recive_data = strMsg;
                    decode_data(recive_data, sokClient.RemoteEndPoint.ToString()); //解析接收数据
                    reloadDataGridView();                   //重载表格数据
                }
                catch (SocketException se)
                {
                    ShowMsg("异常：" + se.Message);
                    // 从 通信套接字 集合中删除被中断连接的通信套接字；  
                    dict.Remove(sokClient.RemoteEndPoint.ToString());
                    // 从通信线程集合中删除被中断连接的通信线程对象；  
                    dictThread.Remove(sokClient.RemoteEndPoint.ToString());
                    // 从列表中移除被中断的连接IP  
                    lbOnline.Items.Remove(sokClient.RemoteEndPoint.ToString());
                    break;
                }
                catch (Exception e)
                {
                    ShowMsg("异常：" + e.Message);
                    // 从 通信套接字 集合中删除被中断连接的通信套接字；
                    dict.Remove(sokClient.RemoteEndPoint.ToString());
                    // 从通信线程集合中删除被中断连接的通信线程对象； 
                    dictThread.Remove(sokClient.RemoteEndPoint.ToString());
                    // 从列表中移除被中断的连接IP  
                    lbOnline.Items.Remove(sokClient.RemoteEndPoint.ToString());
                    break;
                }
            }
        }

        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="str"></param>
        void ShowMsg(string str)
        {
            if (txtMsg.Text != "") { txtMsg.Text += "\r\n"; }
            txtMsg.Text += DateTime.Now.ToString("HH:mm:ss:") + str;
            txtMsg.Select(txtMsg.Text.Length, 0);//将光标设置到最末尾
            txtMsg.ScrollToCaret();  //将滚动条设置到光标处
        }

        /// <summary>
        /// 关闭监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseListen_Click(object sender, EventArgs e)
        {
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes("000004"); // 将要发送的字符串转换成Utf-8字节数组； 
            byte[] arrSendMsg = new byte[arrMsg.Length + 1]; // 上次写的时候把这一段给弄掉了，实在是抱歉哈~ 用来标识发送是数据而不是文件，如果没有这一段的客户端就接收不到消息了~~~ 
            foreach (Socket s in dict.Values)
            {
                s.Send(arrMsg);
            }
            ShowMsg("关闭成功！！");
            socketWatch.Close();
            Environment.Exit(0);
        }

        /// <summary>
        /// 取消车位使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Park_out_Click(object sender, EventArgs e)
        {
            int ParkAddr;
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(txtParkaddr.Text.Trim() + "20"); // 将要发送的字符串转换成Utf-8字节数组； 
            byte[] arrSendMsg = new byte[arrMsg.Length + 1]; // 上次写的时候把这一段给弄掉了，实在是抱歉哈~ 用来标识发送是数据而不是文件，如果没有这一段的客户端就接收不到消息了~~~  
            //arrSendMsg[0] = 0; // 表示发送的是消息数据  
            foreach (Socket s in dict.Values)
            {
                s.Send(arrMsg);
            }
            ParkAddr = Convert.ToInt32(txtParkaddr.Text.Trim());
            Save_operation(ParkAddr, "设置待用");
            Refresh_Data(ParkAddr, 0, "192.168.1.31");
            txtParkaddr.Clear();
            ShowMsg("发送完毕～～～");
        }

        /// <summary>
        /// 检测车位状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Park_check_Click(object sender, EventArgs e)
        {
            int ParkAddr;
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(txtParkaddr.Text.Trim() + "3"); // 将要发送的字符串转换成Utf-8字节数组； 
            byte[] arrSendMsg = new byte[arrMsg.Length + 1]; // 上次写的时候把这一段给弄掉了，实在是抱歉哈~ 用来标识发送是数据而不是文件，如果没有这一段的客户端就接收不到消息了~~~  
            //arrSendMsg[0] = 0; // 表示发送的是消息数据  
            foreach (Socket s in dict.Values)
            {
                s.Send(arrMsg);
            }
            ParkAddr = Convert.ToInt32(txtParkaddr.Text.Trim());
            Save_operation(ParkAddr, "检测车位状态");
            //Save_operation(txtParkaddr.);
            txtParkaddr.Clear();
            ShowMsg("发送完毕～～～");
            
        }

        /// <summary>
        /// 设定车位为已用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Park_get_Click(object sender, EventArgs e)
        {
            int ParkAddr;
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(txtParkaddr.Text.Trim()+"21"); // 将要发送的字符串转换成Utf-8字节数组； 
            byte[] arrSendMsg = new byte[arrMsg.Length + 1]; // 上次写的时候把这一段给弄掉了，实在是抱歉哈~ 用来标识发送是数据而不是文件，如果没有这一段的客户端就接收不到消息了~~~  
            //arrSendMsg[0] = 0; // 表示发送的是消息数据  
            foreach (Socket s in dict.Values)
            {
                s.Send(arrMsg);
            }
            ParkAddr = Convert.ToInt32(txtParkaddr.Text.Trim());
            Save_operation(ParkAddr,"设置已用");
            Refresh_Data(ParkAddr,1,"192.168.1.31");
            txtParkaddr.Clear();
            ShowMsg("发送完毕～～～");
        }

        /// <summary>
        /// 窗体化程序重载窗体数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            createDataBase();
            reloadDataGridView();
        }

        /// <summary>
        /// 重载数据库到表格
        /// </summary>
        private void reloadDataGridView()
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\ParkInformation.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置
            dbPark.Rows.Clear();                //清空表格
            try
            {
                conn.Open();        //打开数据库，若文件不存在会自动创建
                string sql = "SELECT COUNT(*) FROM Park";
                SQLiteCommand cmdQ = new SQLiteCommand(sql,conn);
                int RowCount = Convert.ToInt32(cmdQ.ExecuteScalar());   //获取总车位数
                lbHeadcount.Text = "总车位数：" + RowCount.ToString();
                cmdQ.Dispose();//释放reader使用的资源，防止database is lock异常产生

                sql = "SELECT * FROM Park";
                cmdQ = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmdQ.ExecuteReader();
                while (reader.Read())
                {
                    int index = dbPark.Rows.Add();

                    dbPark.Rows[index].Cells[0].Value = reader.GetInt16(0);
                    dbPark.Rows[index].Cells[1].Value = reader.GetInt32(1);
                    dbPark.Rows[index].Cells[2].Value = reader.GetString(3);
                }
                reader.Dispose();//释放reader使用的资源，防止database is lock异常产生
            }
            catch  (Exception ex)
            {
                output(ex.Message);
            }
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }

        /// <summary>
        /// 新建数据库
        /// </summary>
        private void createDataBase()
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\ParkInformation.db";
            conn = new SQLiteConnection(dbPath);
            try
            {
                conn.Open();
                string sql = "CREATE TABLE IF NOT EXISTS Park(" +       //建表语句
                             "ParkAddr INTEGER PRIMARY KEY," +          //车位地址
                             "state TINYINT," +                         //状态
                             "IP VARCHAR(20)," +                        //中继站IP
                             "time VARCHAR(14));";                      //时间
                SQLiteCommand cmdCreateTable = new SQLiteCommand(sql, conn);
                cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建车位信息表

                sql = "CREATE TABLE IF NOT EXISTS access(" +     //建表语句
                      "ParkAddr INTEGER," +                      //车位地址
                      "state INTEGER,"  +                        //进出状态
                      "CardNum VARCHAR(20)," +                   //卡号
                      "IP VARCHAR(20)," +                        //中继站IP
                      "time VARCHAR(14));";                      //时间
                cmdCreateTable = new SQLiteCommand(sql, conn);
                cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建车位出入信息表

                sql = "CREATE TABLE IF NOT EXISTS operation(" +     //建表语句
                      "ParkAddr INTEGER," +                        //车位地址
                      "mode VARCHAR(20)," +                         //使用操作
                      "time VARCHAR(14));";                         //操作时间
                cmdCreateTable = new SQLiteCommand(sql, conn);
                cmdCreateTable.ExecuteNonQuery();//如果表不存在，创建车位出入信息表
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="log"></param>
        private void output(string log)
        {
            if (txtMsg.Text != "") { txtMsg.Text += "\r\n"; }
            txtMsg.Text += DateTime.Now.ToString("HH:mm:ss ") + log;
            txtMsg.Select(txtMsg.Text.Length, 0);//将光标设置到最末尾
            txtMsg.ScrollToCaret();  //将滚动条设置到光标处
        }

        /// <summary>
        /// 解析接收数据
        /// </summary>
        /// <param name="str"></param>
        private void decode_data(string str,string IP)
        {
            int ParkAddr,state;
            string card;
            string addr_temp, stat_temp,card_temp;
            int IP_lenth = IP.IndexOf(":");
            IP = IP.Remove(IP_lenth);       //提取IP地址
            string data = str;
            int len_set = data.IndexOf("IOSET");
            if(len_set == 0)
            {
                //分割数据
                addr_temp = data.Substring(5);
                card_temp = data.Substring(10);
                stat_temp = data.Substring(18);
                //数据解析
                ParkAddr = Convert.ToInt32(addr_temp.Remove(5));//得到车位地址
                card = card_temp.Remove(8);                     //得到卡号
                state = Convert.ToInt32(stat_temp.Remove(1));   //得到进出状态
                Save_access(ParkAddr,state,card,IP);            //保存进出记录
                Refresh_Data(ParkAddr, state, IP);              //刷新车位信息
            }
            int len_ck = data.IndexOf("CHECK");
            if (len_ck == 0)
            {
                //分割数据
                addr_temp = data.Substring(5);
                stat_temp = data.Substring(10);
                //得到车位地址和状态
                ParkAddr = Convert.ToInt32(addr_temp.Remove(5));
                state = Convert.ToInt32(stat_temp.Remove(1));
                Refresh_Data(ParkAddr,state,IP);
            }
        }

        /// <summary>
        /// 保存进出库记录
        /// </summary>
        private void Save_access(int ParkAddr,int state,string card, string IP)
        {
            byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(ParkAddr.ToString("d5") + state.ToString()); // 将要发送的字符串转换成Utf-8字节数组； 

            //发送回复
            foreach (Socket s in dict.Values)
            {
                s.Send(arrMsg);
            }

            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\ParkInformation.db";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);        //创建数据库实例，指定文件位置
            conn.Open();        //打开数据库，若文件不存在会自动创建

            try
            {
                SQLiteTransaction tran = conn.BeginTransaction();
                SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL指令
                cmd.Transaction = tran;
                cmd.CommandText = "insert into access values(@ParkAddr,@state,@CardNum,@IP,@time)";//设置带参SQL语句
                cmd.Parameters.AddRange(new[] {
                                        new SQLiteParameter("@ParkAddr",ParkAddr),
                                        new SQLiteParameter("@state",state),
                                        new SQLiteParameter("@CardNum",card),
                                        new SQLiteParameter("@IP",IP.ToString()),
                                        new SQLiteParameter("@time",DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分"))
                                        });
                cmd.ExecuteNonQuery();//执行查询
                tran.Commit();//提交
                cmd.Dispose();//释放reader使用的资源，防止database is lock异常产生
                tran.Dispose();//释放reader使用的资源，防止database is lock异常产生
            }
            catch (Exception ex)
            {
                output(ex.Message);
            }
            reloadDataGridView();       //向表格重新添加数据
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }

        /// <summary>
        /// 保存操作记录
        /// </summary>
        private void Save_operation(int ParkAddr,string mode)
        {
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\ParkInformation.db";
            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);        //创建数据库实例，指定文件位置
            conn.Open();        //打开数据库，若文件不存在会自动创建

            try
            {
                SQLiteTransaction tran = conn.BeginTransaction();
                SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL指令
                cmd.Transaction = tran;
                cmd.CommandText = "insert into operation values(@ParkAddr,@mode,@time)";//设置带参SQL语句
                cmd.Parameters.AddRange(new[] {
                                        new SQLiteParameter("@ParkAddr",ParkAddr),
                                        new SQLiteParameter("@mode",mode.ToString()),
                                        new SQLiteParameter("@time",DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分"))
                                        });
                cmd.ExecuteNonQuery();//执行查询
                tran.Commit();//提交
                cmd.Dispose();//释放reader使用的资源，防止database is lock异常产生
                tran.Dispose();//释放reader使用的资源，防止database is lock异常产生
            }
            catch (Exception ex)
            {
                output(ex.Message);
            }
            reloadDataGridView();       //向表格重新添加数据
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }

        /// <summary>
        /// 刷新车位数据
        /// </summary>
        private void Refresh_Data(int ParkAddr,int state,string IP)
        {
            bool isSearch = false;
            int rowsCount = dbPark.Rows.Count;//获取总车位数
            string cellString = dbPark.Rows[rowsCount - 1].Cells[0].Value.ToString().Trim();//初始化为最后一行的用户号
           
            string dbPath = "Data Source =" + Environment.CurrentDirectory + @"\dataBase\ParkInformation.db";

            SQLiteConnection conn = null;
            conn = new SQLiteConnection(dbPath);        //创建数据库实例，指定文件位置
            conn.Open();        //打开数据库，若文件不存在会自动创建
            try
            {
                SQLiteTransaction tran = conn.BeginTransaction();
                SQLiteCommand cmd = new SQLiteCommand(conn);//实例化SQL指令
                cmd.Transaction = tran;
                //查询车位地址是否存在
                for (int i = 0; i < rowsCount; i++)//从1号一直搜索到rowsCount - 1号地址
                {
                    isSearch = false;
                    for (int j = 0; j < rowsCount; j++)//搜索有无第i号车位
                    {
                        cellString = dbPark.Rows[j].Cells[0].Value.ToString().Trim();//得到车位地址
                        if (ParkAddr == int.Parse(cellString))
                        {
                            isSearch = true;//如果搜索到有第i号车位，跳出循环，搜索下一车位
                            break;
                        }
                    }
                }
                if (isSearch == false)//如果没有搜索到i号车位，证明第i号车位空缺
                {
                    cmd.CommandText = "insert into Park values(@ParkAddr,@state,@IP,@time)";//设置带参SQL语句
                    cmd.Parameters.AddRange(new[] {
                                        new SQLiteParameter("@ParkAddr",ParkAddr),
                                        new SQLiteParameter("@state",state),
                                        new SQLiteParameter("@IP",IP.ToString()),
                                        new SQLiteParameter("@time",DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分"))
                                        });
                }
                else
                {
                    cmd.CommandText = "update Park set " +
                                      "state = @state," +
                                      "IP    = @IP," +
                                      "time  = @time "+
                                      "where ParkAddr = @ParkAddr";//设置带参SQL语句
                    cmd.Parameters.AddRange(new[] {
                                        new SQLiteParameter("@ParkAddr",ParkAddr),
                                        new SQLiteParameter("@state",state),
                                        new SQLiteParameter("@IP",IP.ToString()),
                                        new SQLiteParameter("@time",DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分"))
                                        });
                }
                cmd.ExecuteNonQuery();//执行查询
                tran.Commit();//提交
                cmd.Dispose();//释放reader使用的资源，防止database is lock异常产生
                tran.Dispose();//释放reader使用的资源，防止database is lock异常产生
                output(ParkAddr.ToString() + "号车位数据刷新！");
            }
            catch (Exception ex)
            {
                if (ex.Message == "constraint failed\r\nUNIQUE constraint failed: Park.ParkAddr")
                {
                    output("用户号不能与其他用户相同！");
                }
                else
                {
                    output(ex.Message);
                }
            }
            reloadDataGridView();       //向表格重新添加数据
            conn.Close();
            conn.Dispose();//释放reader使用的资源，防止database is lock异常产生
        }
    }
}
