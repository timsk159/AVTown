using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATServer
{
	class CharacterData
	{


        public CharacterData(){}

        public CharacterData(string userName)
        {

        }

        public CharacterData GetFromDB(string userName)
        {


            return new CharacterData();
        }
	}
}
