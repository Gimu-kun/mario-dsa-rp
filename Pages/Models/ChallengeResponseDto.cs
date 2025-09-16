using System;
using System.Collections.Generic;

namespace mario_dsa_rp.Models
{
    public class ChallengeResponseDto
    {
        public string Message { get; set; }
        public ChallengeDto Challenge { get; set; }
        public List<TakenDto> Taken { get; set; }
    }

    public class ChallengeDto
    {
        public string Id { get; set; }
        public string Topic { get; set; }
        public string QuesDataJson { get; set; }
        public string DpBoardJson { get; set; }
        public string ResultItemsJson { get; set; }
        public int MaxCapacity { get; set; }
        public int MaxValue { get; set; }
        public int Difficulty { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<ItemDto> QuesData { get; set; }
        public List<List<int>> DpBoard { get; set; }
        public List<List<int>> DpBoardWithMiss { get; set; }
        public List<string> ResultItems { get; set; }
    }

    public class ItemDto
    {
        public string Id { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }
    }
    
    public class TakenDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int ChallengeType { get; set; }
        public int Mode { get; set; }
        public string ChallengeId { get; set; }
        public int SpendTime { get; set; }
        public int MaxScore { get; set; }
        public int TakenScore { get; set; }
        public string UserAnswer { get; set; }
        public DateTime TakenAt { get; set; }
    }
}
