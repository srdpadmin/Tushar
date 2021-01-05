using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreAssemblies
{
    [Obsolete]
    public class EnumObject
    {

        private int? mValue;

        private string msValue;

        private string mDisplay;

        // Will be used to for taking care of the boolean value in the display grid
        private bool mBoolVal;

        public string EnumStringValue
        {
            get
            {
                return msValue;
            }
            set
            {
                msValue = value;
            }
        }

        public int? EnumValue
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
            }
        }

        public string EnumDisplay
        {
            get
            {
                return mDisplay;
            }
            set
            {
                mDisplay = value;
            }
        }

        public bool EnumBoolValue
        {
            get
            {
                return mBoolVal;
            }
            set
            {
                mBoolVal = value;
            }
        }
    }
}
