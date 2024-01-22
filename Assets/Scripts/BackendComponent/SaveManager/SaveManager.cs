using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Assets.Scripts.Helper;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.DataPersistence.SaveManager
{
    //[SerializeField] private MissionBoardSceneData _missionBoardSceneData; 
    public class SaveManager : ISaveManager
    {
        /// <summary>
        /// Update mission status for chapter after mission is passed.
        /// </summary>
        /// <param name="missionFolderFullPath">Folder path for mission config file in seleted chapter and must be after 'Resources' folder sush as 'MissionConfigs/ChapterX'</param>
        /// <param name="passedMissionName">Mission name that passed.</param>
        public MissionUnlockDetails UpdateMissionStatus(string missionFolderFullPath, MissionUnlockDetails missionStatusDetails, string passedMissionName, string[] missionDependTos)
        {
            // 1) Loop for update status
            foreach (MissionUnlockDetail missionStatusDetail in missionStatusDetails.MissionUnlockDetailList)
            {
                if (missionStatusDetail.MissionName == passedMissionName)
                {
                    missionStatusDetail.IsPass = true;
                }
            }
            // 2) Loop เพื่อปลดล็อก mission ที่เป็น MissionDependTo
            foreach (MissionUnlockDetail missionStatusDetail in missionStatusDetails.MissionUnlockDetailList)
            {
                if (missionStatusDetail.MissionName != passedMissionName)
                {
                    int totalDependencies = missionStatusDetail.MissionDependenciesUnlockDetail.Length;
                    int totalUnlockDependencies = 0;

                    // Update passed mission dependency's status.
                    foreach (MissionDependencyUnlockDetail missionDependency in missionStatusDetail.MissionDependenciesUnlockDetail)
                    {
                        if (missionDependency.MissionName == passedMissionName)
                        {
                            missionDependency.IsPass = true;
                        }
                        if (missionDependency.IsPass)
                        {
                            totalUnlockDependencies++;
                        }
                    }

                    // If all dependency passed.
                    if (totalUnlockDependencies == totalDependencies)
                    {
                        missionStatusDetail.IsUnlock = true;
                    }
                }
            }
            // 3) Save to file
            string savedFileFullPath = Path.Combine(missionFolderFullPath, EnvironmentData.Instance.MissionStatusFileName + EnvironmentData.Instance.MissionStatusDetailFileType);
            SaveMissionStatusToFile(missionStatusDetails, savedFileFullPath);

            return missionStatusDetails;
        }

        private void SaveMissionStatusToFile(MissionUnlockDetails missionStatusDetails,string fullPath)
        {
            string saveData = JsonUtility.ToJson(missionStatusDetails, true);
            File.WriteAllText(fullPath, saveData);
        }
    }
}
