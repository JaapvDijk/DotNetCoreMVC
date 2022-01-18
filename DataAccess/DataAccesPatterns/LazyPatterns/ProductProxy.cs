using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccesPatterns
{
    //Taget type properties should be virtual
    public class ProductProxy : Product
    {
        public override byte[] Picture2
        {
            get 
            {
                if (base.Picture2 == null) 
                {
                    base.Picture2 = ProductPictureService.GetFor("StaringHamsterPicture");
                }

                return base.Picture2;
            }
        }
    }
}
