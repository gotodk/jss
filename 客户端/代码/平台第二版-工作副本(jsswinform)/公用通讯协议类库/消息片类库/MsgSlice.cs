using System;
using System.Text;
using System.Collections;

namespace 公用通讯协议类库.消息片类库
{
    /// <summary>
    /// 消息分片类，用于对消息进行封包、解包。对每个数据拆分为定长（最后一个可能小于定长）的数据段，并存在一个以序列编号为key的Hashtable中。
    /// 主要用于对较大的图片信息进行拆包、封包
    /// </summary>

    [System.Serializable]
    public class MsgSlice
    {
        /// <summary>
        /// 数据片最大长度,一般情况，局域网1000,广域网500
        /// </summary>
        public static long SLICE_MAX_LENGTH = 5;//数据片最大长度,一般情况，局域网1000,广域网500
        /// <summary>
        /// 本次分片唯一ID,用于标识区分不同的消息整体
        /// </summary>
        private long sliceId;//本次分片唯一ID,用于标识区分不同的消息整体
        /// <summary>
        /// 是否最后一片
        /// </summary>
        private bool sliceEnd = false;//是否最后一片
        /// <summary>
        /// 片在整个消息中的索引
        /// </summary>
        private long sliceIndex = 0;//片在整个消息中的索引
        /// <summary>
        /// 片内容
        /// </summary>
        private byte[] sliceContent;//片内容
        /// <summary>
        /// 未知目的
        /// </summary>
        public bool isSimple = false;

        /// <summary>
        /// 传输通道唯一标记(MsgSlice)
        /// </summary>
        public string onlyonefloat = "";

        /// <summary>
        /// 消息分片后的总片数
        /// </summary>
        public long sliceAllnum = 0;


        public MsgSlice()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="msg">消息片内容</param>
        /// <param name="isEnd">是否最后一片</param>
        /// <param name="id">片唯一编号</param>
        /// <param name="index">片在消息中的索引</param>
        public MsgSlice(byte[] msg, bool isEnd, long id, long index, long sliceAllnum_temp)
        {
            if (msg == null || msg.Length == 0)
            {
                return;
            }
            this.sliceEnd = isEnd;
            this.sliceContent = msg;
            this.sliceId = id;
            this.sliceIndex = index;
            this.sliceAllnum = sliceAllnum_temp;
        }

        /// <summary>
        /// 对二进制数据进行分片，并返回含有分片消息类的Hashtable
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static Hashtable splitMsg(byte[] msg)
        {
            if (msg == null || msg.Length == 0)
            {
                return null;
            }
            //定义消息长度
            long len = msg.Length;
            //
            if (len <= MsgSlice.SLICE_MAX_LENGTH)//如果消息小于最大分片量,则仅产生一个哈希表键值
            {
                Hashtable h = new Hashtable();
                h.Add((long)0, new MsgSlice(msg, true, 1, (long)0, (long)1));

                return h;//返回分好片的哈希表
            }
            //定义分片数量
            long sliceNum = 1;
            //
            if (len % MsgSlice.SLICE_MAX_LENGTH == 0)//消息长度刚好可以均匀分片,计算分片数量
            {
                sliceNum = len / MsgSlice.SLICE_MAX_LENGTH;
            }
            else  //不能整除，将分片数量加1
            {
                sliceNum = len / MsgSlice.SLICE_MAX_LENGTH + 1;
            }
            Hashtable ms = new Hashtable();
            long id = 1; //片唯一编号
            long i = 0;
            //开始对消息进行分片并添加到哈希表
            for (; i < sliceNum - 1; i++)
            {
                byte[] tByte = MsgSlice.byteCopy(msg, i * MsgSlice.SLICE_MAX_LENGTH, MsgSlice.SLICE_MAX_LENGTH);
                ms.Add(i, new MsgSlice(tByte, false, id, i, (long)sliceNum));
            }
            //产生最后一片消息加入哈希表
            byte[] teByte = MsgSlice.byteCopy(msg, i * MsgSlice.SLICE_MAX_LENGTH, MsgSlice.SLICE_MAX_LENGTH);
            ms.Add(i, new MsgSlice(teByte, true, id, i, (long)sliceNum));
            return ms;//返回分好片的哈希表
        }

        //对字符串进行分片（重载）,并返回分片Hashtable,用于对文字消息分片
        public static Hashtable splitMsg(string msg)
        {
            return MsgSlice.splitMsg(System.Text.ASCIIEncoding.UTF8.GetBytes(msg));
        }

        /// <summary>
        /// 对分片数据进行重组
        /// </summary>
        /// <param name="h">含有消息片类的哈希表</param>
        /// <returns></returns>
        public static byte[] uniteMsg(Hashtable h)
        {
            byte[] result = null;
            for (int i = 0; i < h.Count; i++)
            {
                if (h[(long)i] != null)
                {
                    result = MsgSlice.byteAdd(result, ((MsgSlice)h[(long)i]).sliceContent);
                    //Console.WriteLine("加入数据" + i);
                }
                else
                {
                    //Console.WriteLine("忽略空数据" + i);
                }
            }
            return result;
        }

        /// <summary>
        /// 累加字节
        /// </summary>
        /// <param name="bt1">原始字节</param>
        /// <param name="bt2">需要累加的字节</param>
        /// <returns></returns>
        public static byte[] byteAdd(byte[] bt1, byte[] bt2)
        {
            if (bt1 == null)
            {
                return bt2;
            }
            if (bt2 == null)
            {
                return bt1;
            }

            long len1 = bt1.Length;
            long len2 = bt2.Length;

            byte[] result = new byte[len1 + len2];
            bt1.CopyTo(result, 0);

            for (long i = 0; i < len2; i++)
            {
                result[len1 + i] = bt2[i];
            }
            return result;
        }

        /// <summary>
        /// 根据分片索引计算并返回消息分片
        /// </summary>
        /// <param name="source">消息</param>
        /// <param name="index">分片索引</param>
        /// <param name="len">消息片最大长度</param>
        /// <returns></returns>
        public static byte[] byteCopy(byte[] source, long index, long len)
        {
            byte[] dest;
            if (index < 0)
            {
                return null;
            }
            long sLen = source.Length;
            if (sLen < index + len)
            {
                dest = new byte[sLen - index];
            }
            else
            {
                dest = new byte[len];
            }
            long i = 0;
            for (; i < dest.Length; i++)
            {
                dest[i] = source[index + i];
            }
            return dest;
        }

        /// <summary>
        /// 得到消息片索引
        /// </summary>
        /// <returns></returns>
        public long getSliceIndex()
        {
            return this.sliceIndex;
        }

        /// <summary>
        /// 得到本次分片唯一ID,用于标识区分不同的消息整体
        /// </summary>
        /// <returns></returns>
        public long getSliceId()
        {
            return this.sliceId;
        }

        /// <summary>
        /// 得到是否最后一片
        /// </summary>
        /// <returns></returns>
        public bool isEndSlice()
        {
            return this.sliceEnd;
        }

        /// <summary>
        /// 得到分片后的消息内容
        /// </summary>
        /// <returns></returns>
        public byte[] getSliceContent()
        {
            return this.sliceContent;
        }

        /// <summary>
        /// 得到分片后的总片数
        /// </summary>
        /// <returns></returns>
        public long getsliceAllnum()
        {
            return this.sliceAllnum;
        }
    }
}
