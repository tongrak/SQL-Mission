using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Assets.Scripts.Helper;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.DataPersistence.SaveManager
{
    public class SaveManager : ISaveManager
    {
        /// <summary>
        /// Update mission status for chapter after mission is passed.
        /// </summary>
        /// <param name="missionFolderFullPath">Folder path for mission config file in seleted chapter and must be after 'Resources' folder sush as 'MissionConfigs/ChapterX'</param>
        /// <param name="passedMissionName">Mission name that passed.</param>
        public void UpdateMissionStatus(string missionFolderFullPath, string passedMissionName, string[] missionDependTos)
        {
            // 1) Get mission status detail
            TextAsset missionStatusFile = Resources.Load<TextAsset>(missionFolderFullPath.Split(new string[] { "Resources/" }, StringSplitOptions.None)[1] + '/' + EnvironmentData.Instance.MissionStatusFileName);
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
            // 4) Save to file
            string saveData = JsonUtility.ToJson(missionStatusDetails, true);
            string savedFileFullPath = Path.Combine(missionFolderFullPath, EnvironmentData.Instance.MissionStatusFileName + EnvironmentData.Instance.MissionStatusDetailFileType);

            File.WriteAllText(savedFileFullPath, saveData);
        }
    }
}
