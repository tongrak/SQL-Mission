using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.BackendComponent.SaveManager
{
    public class SaveManager : ISaveManager
    {
        public void UpdateMissionStatus(string missionFolderPath, string passedMissionName)
        {
            throw new NotImplementedException();
            // ในระหว่างที่วน loop ใน mission dependencies ให้เก็บจำนวน mission ที่ผ่านไว้ด้วย จะได้ไม่ต้องวนลูปเพื่อนับจำนวน mission ที่ผ่านใหม่อีกรอบหลังจาก update ตัว dependency เสร็จ
        }
    }
}
