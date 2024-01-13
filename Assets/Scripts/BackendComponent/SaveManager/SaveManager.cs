using Assets.Scripts.BackendComponent.Model;
using Assets.Scripts.Helper;
using System;
using UnityEngine;

namespace Assets.Scripts.BackendComponent.SaveManager
{
    public class SaveManager : ISaveManager
    {
        /// <summary>
        /// Update mission status for chapter after mission is passed.
        /// </summary>
        /// <param name="missionFolderPath">Folder path for mission config file in seleted chapter and must be after 'Resources' folder sush as 'MissionConfigs/ChapterX'</param>
        /// <param name="passedMissionName">Mission name that passed.</param>
        public void UpdateMissionStatus(string missionFolderPathAfterResources, string passedMissionName, string[] missionDependTos)
        {
            // 1) Get mission status detail
            TextAsset missionStatusFile = Resources.Load<TextAsset>(missionFolderPathAfterResources + '/' + EnvironmentData.Instance.MissionStatusFileName);
            MissionUnlockDetails missionStatusDetails = JsonUtility.FromJson<MissionUnlockDetails>(missionStatusFile.text);
            // 2) Loop for update status
            foreach (MissionUnlockDetail missionStatusDetail in missionStatusDetails.MissionUnlockDetailList)
            {
                if (missionStatusDetail.MissionName == passedMissionName)
                {
                    missionStatusDetail.IsPass = true;
                }
            }
            // 3) Loop เพื่อปลดล็อก mission ที่เป็น MissionDependTo
            foreach (MissionUnlockDetail missionStatusDetail in missionStatusDetails.MissionUnlockDetailList)
            {
                if (missionStatusDetail.MissionName != passedMissionName)
                {
                    int totalDependencies = missionStatusDetail.MissionDependenciesUnlockDetail.Length;
                    int totalUnlockDependencies = 0;

                    // Update passed mission dependency's status.
                    foreach (MissionDependencyUnlockDetail missionDependencyDetail in missionStatusDetail.MissionDependenciesUnlockDetail)
                    {
                        if (missionStatusDetail.MissionName == passedMissionName)
                        {
                            missionStatusDetail.IsPass = true;
                        }
                        else
                        {
                            if (missionStatusDetail.IsPass)
                            {
                                totalUnlockDependencies++;
                            }
                        }
                    }

                    // If all dependency passed.
                    if (totalUnlockDependencies == totalDependencies)
                    {
                        missionStatusDetail.IsUnlock = true;
                    }
                }
            }
            // 4) Save to file
            string saveData = JsonUtility.ToJson(missionStatusDetails);
            System.IO.File.WriteAllText(missionFolderPathAfterResources + "/" + EnvironmentData.Instance.MissionStatusFileName + EnvironmentData.Instance.MissionStatusDetailFileType, saveData);
        }
    }
}
