using Assets.Scripts.DataPersistence.MissionStatusDetail;

namespace Assets.Scripts.DataPersistence.SaveManager
{
    public interface ISaveManager
    {
        /// <summary>
        /// Update mission status for chapter after mission is passed.
        /// </summary>
        /// <param name="missionFolderFullPath">Inside path must contain 'Resources/' folder such as "D:X/X/X/Resources/X/X/Chapter1"</param>
        /// <param name="passedMissionID">Mission ID that passed.</param>
        MissionUnlockDetails UpdateMissionStatus(string missionFolderFullPath, MissionUnlockDetails missionStatusDetails, int passedMissionID, int[] missionDependTos);

        ///// <summary>
        ///// Update chapter status.
        ///// </summary>
        ///// <param name="chapterFolderPath">Folder path for chapter config file in seleted chapter and must be after 'Resources' folder sush as 'MissionConfigs/ChapterX'</param>
        ///// <param name="passedChapterName">Chapter name that passed.</param>
        //void UpdateChapterStatus(string chapterFolderPath, string passedChapterName);
    }
}
