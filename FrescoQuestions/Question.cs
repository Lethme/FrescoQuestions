using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Answer = System.Tuple<System.String, System.String>;

namespace Tree
{
    public enum AnswerType
    {
        Left,
        Right
    }
    public class Question : IComparable<Question>
    {
        [JsonProperty("Question")]
        public String QuestionText { get; private set; }
        [JsonProperty("Answers")]
        public Answer QuestionAnswers { get; private set; }
        [JsonProperty("Chosen Answer"), JsonIgnore]
        public AnswerType ChosenAnswer { get; private set; }
        [JsonProperty("ID"), JsonIgnore]
        public int QuestionID { get; set; }
        [JsonProperty("Score")]
        public int Score { get; set; }
        [JsonProperty("Right Answer")]
        public AnswerType RightAnswer { get; set; } = AnswerType.Left;
        public String ChosenAnswerText 
        {
            get
            {
                switch (ChosenAnswer)
                {
                    case AnswerType.Left: return QuestionAnswers.Item1;
                    case AnswerType.Right: return QuestionAnswers.Item2;
                    default: return "Дед";
                }
            }
        }
        public String RightAnswerText 
        {
            get
            {
                switch (RightAnswer)
                {
                    case AnswerType.Left: return QuestionAnswers.Item1;
                    case AnswerType.Right: return QuestionAnswers.Item2;
                    default: return "Дед";
                }
            }
        }
        public String WrongAnswetText
        {
            get
            {
                switch (RightAnswer)
                {
                    case AnswerType.Left: return QuestionAnswers.Item2;
                    case AnswerType.Right: return QuestionAnswers.Item1;
                    default: return "Дед";
                }
            }
        }
        public Question() {  }
        public Question(String question, Answer answers, int score, AnswerType answerType)
        {
            QuestionText = question;
            QuestionAnswers = answers;
            Score = score;
            RightAnswer = answerType;
        }
        public Question(String question, (String, String) answers, int score, AnswerType answerType)
        {
            QuestionText = question;
            QuestionAnswers = answers.ToTuple();
            Score = score;
            RightAnswer = answerType;
        }
        public Question(String question, String firstAnswer, String secondAnswer, int score, AnswerType answerType)
        {
            QuestionText = question;
            QuestionAnswers = (firstAnswer, secondAnswer).ToTuple();
            Score = score;
            RightAnswer = answerType;
        }
        public int SelectAnswer(AnswerType answerType)
        {
            switch (answerType)
            {
                case AnswerType.Left: { ChosenAnswer = AnswerType.Left; break; }
                case AnswerType.Right: { ChosenAnswer = AnswerType.Right; break; }
                default: throw new ArgumentOutOfRangeException();
            }

            if (ChosenAnswer == RightAnswer) return Score;
            return 0;
        }
        public int CompareTo(Question other) => QuestionText.CompareTo(other.QuestionText);
        public override string ToString() => $"{QuestionText}\n{QuestionAnswers.Item1} / {QuestionAnswers.Item2}";
        public static Question Create(String question, Answer answers, int score, AnswerType answerType) => new Question(question, answers, score, answerType);
        public static Question Create(String question, (String, String) answers, int score, AnswerType answerType) => new Question(question, answers, score, answerType);
        public static Question Create(String question, String firstAnswer, String secondAnswer, int score, AnswerType answerType) => new Question(question, firstAnswer, secondAnswer, score, answerType);
    }
    public class QuestionPath
    {
        public static List<String> TestRank = new List<string>()
        {
            "Бублик с дыркой",
            "Валенок",
            "Вафельница",
            "Офисный планктон",
            "Уверенный пользователь ПК",
            "Студент ВПИ",
            "Искусственный интеллект",
            "Вы уже настолько преисполнились..."
        };
        private BinaryTree<Question> QuestionTree { get; set; }
        public Question CurrentQuestion => CurrentNode.Data;
        private BinaryTreeNode<Question> CurrentNode { get; set; }
        public IEnumerable<Question> QuestionList => QuestionTree.Pass(PassType.FloorsOrder);
        public int TotalScore { get; private set; }
        public IEnumerable<Question> QuestionPathList
        { 
            get
            {
                var questionList = new List<Question>();
                var Node = CurrentNode;
                do
                {
                    questionList.Add(Node.Data);
                    Node = Node.Parent;
                } while (Node != null);

                return questionList.Reverse<Question>();
            }
        }
        public QuestionPath(BinaryTree<Question> questionTree)
        {
            QuestionTree = questionTree;
            QuestionTree.Mix();
            CurrentNode = questionTree.Root;
        }
        public QuestionPath(params Question[] questions)
        {
            QuestionTree = BinaryTree.Create(questions);
            QuestionTree.Mix();
            CurrentNode = QuestionTree.Root;
        }
        public QuestionPath(IEnumerable<Question> questions)
        {
            QuestionTree = BinaryTree.Create(questions);
            QuestionTree.Mix();
            CurrentNode = QuestionTree.Root;
        }
        public QuestionPath(String FileName)
        {
            try
            {
                using var Reader = new StreamReader(FileName);

                QuestionTree = BinaryTree.Create
                (
                    JsonConvert.DeserializeObject<List<Question>>(Reader.ReadToEnd())
                );
                QuestionTree.Mix();
                CurrentNode = QuestionTree.Root;
            }
            catch { }
        }
        public bool NextQuestion()
        {
            if (CurrentQuestion.ChosenAnswer == 0 && CurrentNode.Left != null)
            {
                CurrentNode = CurrentNode.Left;
                return true;
            }

            if (CurrentQuestion.ChosenAnswer != 0 && CurrentNode.Right != null)
            {
                CurrentNode = CurrentNode.Right;
                return true;
            }

            return false;
        }
        public Question GetQuestion(String questionText) => QuestionList.Where(item => item.QuestionText == questionText).First();
        public void Reset()
        {
            QuestionTree.Mix();
            CurrentNode = QuestionTree.Root;
            TotalScore = 0;
        }
        public string WhoAmI()
        {
            switch (TotalScore)
            {
                case 0: { return TestRank[0]; }
                case 1: { return TestRank[1]; }
                case 2: { return TestRank[2]; }
                case 3: { return TestRank[3]; }
                case 4: { return TestRank[4]; }
                case 5: { return TestRank[5]; }
                case 6: { return TestRank[6]; }
                case 7: { return TestRank[7]; }
                default: return "Дед";
            }
        }
        public IEnumerable<Question> PathToSolution(Question question, String rank)
        {
            var count = TestRank.IndexesWhere(x => x == rank).First();
            var Node = QuestionTree.FindNodeByValue(question);
            
            while (Node != null)
            {
                if (count-- > 0)
                {
                    yield return Node.Data;
                    switch (Node.Data.RightAnswer)
                    {
                        case AnswerType.Left: Node = Node.Left; break;
                        case AnswerType.Right: Node = Node.Right; break;
                    }
                }
                else
                {
                    yield return Node.Data;
                    switch (Node.Data.RightAnswer)
                    {
                        case AnswerType.Left: Node = Node.Right; break;
                        case AnswerType.Right: Node = Node.Left; break;
                    }
                }
            }
        }
        public IEnumerable<Question> PathFromRoot(Question question)
        {
            var Node = QuestionTree.FindNodeByValue(question);
            var NodeList = new List<BinaryTreeNode<Question>>();
            var List = new List<Question>();
            
            while (Node != null)
            {
                NodeList.Add(Node);
                Node = Node.Parent;
            }

            NodeList.Reverse();

            for (var i = 0; i < NodeList.Count - 1; i++)
            {
                if (NodeList[i].Left == NodeList[i + 1]) NodeList[i].Data.SelectAnswer(AnswerType.Left);
                if (NodeList[i].Right == NodeList[i + 1]) NodeList[i].Data.SelectAnswer(AnswerType.Right);
            }

            return NodeList.Select(node => node.Data);
        }
        public void SelectAnswer(AnswerType answerType) => TotalScore += CurrentQuestion.SelectAnswer(answerType);
        public static QuestionPath Create(BinaryTree<Question> questionTree) => new QuestionPath(questionTree);
        public static QuestionPath Create(params Question[] questions) => new QuestionPath(questions);
        public static QuestionPath Create(IEnumerable<Question> questions) => new QuestionPath(questions);
        public static QuestionPath Create(String FileName) => new QuestionPath(FileName);
    }
}
