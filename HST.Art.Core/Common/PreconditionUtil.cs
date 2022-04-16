using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZT.SMS.Core
{
    public class PreconditionUtil
    {
        public static void checkArgument(bool expression, string errorCode, string parame)
        {
            if (!expression)
            {
                throw new BizException(errorCode, parame);
            }
        }

        public static void checkNotNull<T>(T reference, string errorCode, string parame) where T : class
        {
            if (reference == null)
            {
                throw new BizException(errorCode, parame);
            }
        }

        public static void checkArgument(bool expression, string msg)
        {
            if (!expression)
            {
                throw new BizException((int)ErrorCode.ParameterNull+"", msg);
            }
        }

        public static void checkNotNull(object reference, string msg)
        {
            if (reference == null)
            {
                throw new BizException((int)ErrorCode.ParameterNull + "", msg);
            }
        }

    }
}
