﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApplication
{
    public class Utilies
    {
        public enum InvoiceStatus
        {
            WaitingApproval = 1,Checked,CheckPending,CheckRejected,Approved,ApprovePending,ApproveRejected,Rejected
        }
    }
}
