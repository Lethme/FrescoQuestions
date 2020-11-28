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
        public int CompareTo(Question other) => QuestionID.CompareTo(other.QuestionID);
        public override string ToString() => $"{QuestionText}\n{QuestionAnswers.Item1} / {QuestionAnswers.Item2}";
        public static Question Create(String question, Answer answers, int score, AnswerType answerType) => new Question(question, answers, score, answerType);
        public static Question Create(String question, (String, String) answers, int score, AnswerType answerType) => new Question(question, answers, score, answerType);
        public static Question Create(String question, String firstAnswer, String secondAnswer, int score, AnswerType answerType) => new Question(question, firstAnswer, secondAnswer, score, answerType);
    }
    public class QuestionPath
    {
        private BinaryTree<Question> QuestionTree { get; set; }
        public Question CurrentQuestion => CurrentNode.Data;
        private BinaryTreeNode<Question> CurrentNode { get; set; }
        public int TotalCount { get; private set; }
        public IEnumerable<Question> QuestionPathList => ReversedQuestionPathList.Reverse();
        private IEnumerable<Question> ReversedQuestionPathList
        {
            get
            {
                var Node = CurrentNode;
                do
                {
                    yield return Node.Data;
                    Node = Node.Parent;
                } while (Node != null);
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
        public void SelectAnswer(AnswerType answerType) => TotalCount += CurrentQuestion.SelectAnswer(answerType);
        public static QuestionPath Create(BinaryTree<Question> questionTree) => new QuestionPath(questionTree);
        public static QuestionPath Create(params Question[] questions) => new QuestionPath(questions);
        public static QuestionPath Create(IEnumerable<Question> questions) => new QuestionPath(questions);
        public static QuestionPath Create(String FileName) => new QuestionPath(FileName);
    }
}
