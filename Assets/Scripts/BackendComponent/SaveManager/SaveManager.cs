using Assets.Scripts.BackendComponent.Model;
using Assets.Scripts.Helper;
using System;
using System.IO;
using System.Threading;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Assets.Scripts.BackendComponent.SaveManager
{
    public class SaveManager : ISaveManager
    {
        private FileSystemWatcher _fileWatcher;

        //public SaveManager()
        //{
        //    //_fileWatcher = fileWatcher;
        //    _InitiateWatcher();
        //}

        //private void _InitiateWatcher()
        //{
        //    _fileWatcher = new FileSystemWatcher(Application.dataPath + "/Resources/" + EnvironmentData.Instance.MissionConfigRootFolder + "/" + "Chapter1", EnvironmentData.Instance.MissionStatusFileName + EnvironmentData.Instance.MissionStatusDetailFileType);
        //    _fileWatcher.NotifyFilter = NotifyFilters.CreationTime
        //             | NotifyFilters.LastWrite
        //             | NotifyFilters.Size;

        //    _fileWatcher.Changed += TestChangedEvent;

        //    _fileWatcher.EnableRaisingEvents = true;
        //}

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
            string savedFileFullPath = Path.Combine(Application.dataPath, "Resources", missionFolderPathAfterResources, EnvironmentData.Instance.MissionStatusFileName + EnvironmentData.Instance.MissionStatusDetailFileType);

            File.WriteAllText(savedFileFullPath, saveData);
        }

        //private void TestChangedEvent(object sender, FileSystemEventArgs e)
        //{
        //    Debug.Log("File changed.");
        //}
    }
}
