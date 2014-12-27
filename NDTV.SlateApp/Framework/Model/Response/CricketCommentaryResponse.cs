using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Response for cricket commentary
    /// </summary>
    public class CricketCommentaryResponse : Response
    {
        /// <summary>
        /// commentary list
        /// </summary>
        private List<CricketCommentaryItem> commentaryList;

        /// <summary>
        /// Commentary list
        /// </summary>
        public ObservableCollection<CricketCommentaryItem> CommentaryList
        {
            get
            {
                return (null != commentaryList && commentaryList.Count != 0) ? new ObservableCollection<CricketCommentaryItem>(commentaryList) : new ObservableCollection<CricketCommentaryItem>();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">string- response message</param>
        public CricketCommentaryResponse(string responseMessage)
            : base(responseMessage)
        {
            Parse();
        }

        /// <summary>
        /// Parse the cricket commentary
        /// </summary>
        public void Parse()
        {
            commentaryList = new List<CricketCommentaryItem>();
            CricketCommentaryItem commentaryEntity;
            if (false == string.IsNullOrWhiteSpace(responseMessage))
            {
                if (responseMessage.Contains(Constants.Constant.CommentaryLiteral))
                {
                    responseMessage = responseMessage.Substring(Constants.Constant.CommentaryLiteral.Length);
                    responseMessage = HttpHelper.LinkDecode(responseMessage);
                    if (responseMessage.Contains(Constants.Constant.FullStop))
                    {
                        responseMessage = responseMessage.Remove(responseMessage.LastIndexOf(Constants.Constant.FullStop, StringComparison.Ordinal));
                    }
                    string[] commentaryArray;
                    if (responseMessage.Contains(Constants.Constant.PipeSeparator))
                    {
                        commentaryArray = responseMessage.Split('|');
                        foreach (string perBallCommentary in commentaryArray)
                        {
                            string[] ballCommentary;
                            if (perBallCommentary.Contains(Constants.Constant.Tilde))
                            {
                                commentaryEntity = new CricketCommentaryItem();
                                ballCommentary = perBallCommentary.Split('~');
                                if (ballCommentary.Length >= 1 && null != ballCommentary[0] && ballCommentary[0].Equals("nb"))
                                {
                                    commentaryEntity.BallEvent = string.Empty;
                                    commentaryEntity.BallStatus = ballCommentary[0];
                                    if (ballCommentary.Length >= 3 && null != ballCommentary[2])
                                    {
                                        commentaryEntity.ActualCommentary = ballCommentary[2];
                                    }
                                }
                                else
                                {
                                    if (ballCommentary.Length >= 1 && null != ballCommentary[0])
                                    {
                                        commentaryEntity.BallStatus = ballCommentary[0];
                                    }
                                    if (ballCommentary.Length >= 2 && null != ballCommentary[1])
                                    {
                                        commentaryEntity.OverNumber = ballCommentary[1];
                                    }
                                    if (ballCommentary.Length >= 3 && null != ballCommentary[2])
                                    {
                                        commentaryEntity.ActualCommentary = ballCommentary[2];
                                    }
                                    if (ballCommentary.Length >= 4 && null != ballCommentary[3])
                                    {
                                        commentaryEntity.BowlerToBatsmanDetails = ballCommentary[3];
                                    }
                                    if (ballCommentary.Length >= 5 && null != ballCommentary[4])
                                    {
                                        commentaryEntity.ActualCommentary = ballCommentary[4];
                                    }
                                }
                                commentaryList.Add(commentaryEntity);
                            }
                        }
                    }
                }
            }
        }
    }
}
