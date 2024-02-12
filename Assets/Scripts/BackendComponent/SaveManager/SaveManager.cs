using Assets.Scripts.DataPersistence.ChapterStatusDetail;
using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Assets.Scripts.Helper;
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
        /// <param name="passedMissionID">Mission ID that passed.</param>
        public MissionUnlockDetails UpdateMissionStatus(string missionFolderFullPath, MissionUnlockDetails missionStatusDetails, int passedMissionID)
        {
            // 1) Loop for update status
            foreach (MissionUnlockDetail missionStatusDetail in missionStatusDetails.MissionUnlockDetailList)
            {
                if (missionStatusDetail.MissionID == passedMissionID)
                {
                    missionStatusDetail.IsPass = true;
                }
            }
            // 2) Loop for unlock other mission ซึ่งถูก passed mission ล็อกไว้
            foreach (MissionUnlockDetail missionStatusDetail in missionStatusDetails.MissionUnlockDetailList)
            {
                // Mission have dependency and not passed mission
                if (missionStatusDetail.MissionID != passedMissionID && missionStatusDetail.MissionDependenciesUnlockDetail?.Length > 0)
                {
                    int totalDependencies = missionStatusDetail.MissionDependenciesUnlockDetail.Length;
                    int totalUnlockDependencies = 0;

                    // Update passed mission dependency's status.
                    foreach (MissionDependencyUnlockDetail missionDependency in missionStatusDetail.MissionDependenciesUnlockDetail)
                    {
                        if (missionDependency.MissionID == passedMissionID)
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
            string savedFileFullPath = Path.Combine(missionFolderFullPath, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType);
            _SaveMissionStatusToFile(missionStatusDetails, savedFileFullPath);

            return missionStatusDetails;
        }

        private void _SaveMissionStatusToFile(MissionUnlockDetails missionStatusDetails, string fullPath)
        {
            string saveData = JsonUtility.ToJson(missionStatusDetails, true);
            File.WriteAllText(fullPath, saveData);
        }

        public ChapterStatusDetails UpdateChapterStatus(string chapterFolderFullPath, ChapterStatusDetails chapterStatusDetails, int passedChapterID, bool saveToFile)
        {
            // 1) Loop for update status
            foreach (ChapterStatusDetail.ChapterStatusDetail chapterStatusDetail in chapterStatusDetails.ChapterStatusDetailList)
            {
                if (chapterStatusDetail.ChatperID == passedChapterID)
                {
                    chapterStatusDetail.IsPass = true;
                }
            }
            // 2) Loop for unlock other chapter ซึ่งถูก passed chapter ล็อกไว้
            foreach (ChapterStatusDetail.ChapterStatusDetail chapterStatusDetail in chapterStatusDetails.ChapterStatusDetailList)
            {
                // Chapter have dependency and not passed chapter
                if (chapterStatusDetail.ChatperID != passedChapterID && chapterStatusDetail.ChapterDependenciesStatusDetail?.Length > 0)
                {
                    int totalDependencies = chapterStatusDetail.ChapterDependenciesStatusDetail.Length ;
                    int totalUnlockDependencies = 0;

                    // Update passed chapter dependency's status.
                    foreach (ChapterDependenciesStatusDetail chapterDependency in chapterStatusDetail.ChapterDependenciesStatusDetail)
                    {
                        if (chapterDependency.ChapterID == passedChapterID)
                        {
                            chapterDependency.IsPass = true;
                        }
                        if (chapterDependency.IsPass)
                        {
                            totalUnlockDependencies++;
                        }
                    }

                    // If all dependency passed.
                    if (totalUnlockDependencies == totalDependencies)
                    {
                        chapterStatusDetail.IsUnlock = true;
                    }
                }
            }
            // 3) Save to file
            if (saveToFile)
            {
                string savedFileFullPath = Path.Combine(chapterFolderFullPath, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType);
                _SaveChapterStatusToFile(chapterStatusDetails, savedFileFullPath);
            }

            return chapterStatusDetails;
        }

        private void _SaveChapterStatusToFile(ChapterStatusDetails missionStatusDetails, string fullPath)
        {
            string saveData = JsonUtility.ToJson(missionStatusDetails, true);
            File.WriteAllText(fullPath, saveData);
        }
    }
}
