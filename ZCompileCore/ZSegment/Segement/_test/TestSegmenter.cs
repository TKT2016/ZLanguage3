﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JiebaNet.Segmenter.Common;
using NUnit.Framework;

namespace JiebaNet.Segmenter.Tests
{
    [TestFixture]
    public class TestSegmenter
    {
        private string[] GetTestSentences()
        {
            return File.ReadAllLines(@"Cases\jieba_test.txt");
        }

        [TestCase]
        public void TestGetDag()
        {
            var seg = new JiebaSegmenter();
            var dag = seg.GetDag("语言学家参加学术会议");
            foreach (var key in dag.Keys.ToList().OrderBy(k => k))
            {
                Console.Write("{0}: ", key);
                foreach (var i in dag[key])
                {
                    Console.Write("{0} ", i);
                }
                Console.WriteLine();
            }
        }

        [TestCase]
        public void TestCalc()
        {
            var s = "语言学家参加学术会议";
            var seg = new JiebaSegmenter();
            var dag = seg.GetDag(s);
            var route = seg.Calc(s, dag);
            foreach (var key in route.Keys.ToList().OrderBy(k => k))
            {
                Console.Write("{0}: ", key);
                var pair = route[key];
                Console.WriteLine("({0}, {1})", pair.Freq, pair.Key);
            }
        }

        [TestCase]
        public void TestCutDag()
        {
            var s = "语言学家去参加了那个学术会议";
            var seg = new JiebaSegmenter();
            var words = seg.CutDag(s);
            foreach (var w in words)
            {
                Console.WriteLine(w);
            }
        }

        [TestCase]
        public void TestCutDagWithoutHmm()
        {
            var s = "语言学家去参加了那个学术会议";
            var seg = new JiebaSegmenter();
            var words = seg.CutDagWithoutHmm(s);
            foreach (var w in words)
            {
                Console.WriteLine(w);
            }
        }

        #region Jieba Python Test Cases

        [TestCase]
        public void TestCut()
        {
            TestCutFunction((new JiebaSegmenter()).Cut, false, true, @"Cases\accurate_hmm.txt");
        }

        [TestCase]
        public void TestCutAll()
        {
            TestCutFunction((new JiebaSegmenter()).Cut, true, false, @"Cases\cut_all.txt");
        }

        [TestCase]
        public void TestCutWithoutHmm()
        {
            TestCutFunction((new JiebaSegmenter()).Cut, false, false, @"Cases\accurate_no_hmm.txt");
        }

        [TestCase]
        public void TestCutForSearch()
        {
            TestCutSearchFunction((new JiebaSegmenter()).CutForSearch, true, @"Cases\cut_search_hmm.txt");
        }

        [TestCase]
        public void TestCutForSearchWithoutHmm()
        {
            TestCutSearchFunction((new JiebaSegmenter()).CutForSearch, false, @"Cases\cut_search_no_hmm.txt");
        }

        #endregion

        [TestCase]
        public void TestTokenize()
        {
            var seg = new JiebaSegmenter();
            seg.AddWord("机器学习");
            seg.AddWord("自然语言处理");
            foreach (var token in seg.Tokenize("小明最近在学习机器学习、自然语言处理、云计算和大数据"))
            {
                Console.WriteLine(token);
            }

            foreach (var token in seg.Tokenize("小明最近在学习机器学习、自然语言处理、云计算和大数据", TokenizerMode.Search))
            {
                Console.WriteLine(token);
            }
        }

        private static void TestCutThenPrint(JiebaSegmenter segmenter, string s)
        {
            Console.WriteLine(string.Join("/ ", segmenter.Cut(s)));
        }

        [TestCase]
        public void TestAddWord()
        {
            var seg = new JiebaSegmenter();
            var s = "小明最近在学习机器学习和自然语言处理";

            var segments = seg.Cut(s);
            Assert.That(segments, Contains.Item("机器"));
            Assert.That(segments, Contains.Item("学习"));

            seg.AddWord("机器学习");
            segments = seg.Cut(s);
            Assert.That(segments, Contains.Item("机器学习"));
            Assert.That(segments, Is.Not.Contains("机器"));
        }

        [TestCase]
        public void TestCutSpecialWords()
        {
            var seg = new JiebaSegmenter();
            seg.AddWord(".NET");
            seg.AddWord("U.S.A.");
            
            var s = ".NET平台是微软推出的, U.S.A.是美国的简写";

            var segments = seg.Cut(s);
            foreach (var segment in segments)
            {
                Console.WriteLine(segment);
            }

            seg.LoadUserDict(@"Resources\user_dict.txt");
            s = "Steve Jobs重新定义了手机";
            segments = seg.Cut(s);
            foreach (var segment in segments)
            {
                Console.WriteLine(segment);
            }

            s = "我们所熟悉的一个版本是Mac OS X 10.11 EI Capitan，在2015年推出。";
            segments = seg.Cut(s);
            foreach (var segment in segments)
            {
                Console.WriteLine(segment);
            }
        }

        [TestCase]
        [Ignore("")]
        public void TestCutAllSpecialWords()
        {
            // TODO: Enable this test case after confirming with jieba py.
            var seg = new JiebaSegmenter();
            seg.AddWord(".NET");
            seg.AddWord("U.S.A.");
            seg.AddWord("Steve Jobs");
            seg.AddWord("Mac OS X");

            var s = ".NET平台是微软推出的, U.S.A.是美国的简写";
            var segments = seg.Cut(s);
            Console.WriteLine("Cut: {0}", string.Join("/ ", segments));
            segments = seg.Cut(s, cutAll: true);
            Console.WriteLine("Cut All: {0}", string.Join("/ ", segments));

            s = "Steve Jobs重新定义了手机";
            segments = seg.Cut(s);
            Console.WriteLine("Cut: {0}", string.Join("/ ", segments));
            segments = seg.Cut(s, cutAll: true);
            Console.WriteLine("Cut All: {0}", string.Join("/ ", segments));

            s = "我们所熟悉的一个版本是Mac OS X 10.11 EI Capitan，在2015年推出。";

            segments = seg.Cut(s);
            Console.WriteLine("Cut: {0}", string.Join("/ ", segments));
            segments = seg.Cut(s, cutAll: true);
            Console.WriteLine("Cut All: {0}", string.Join("/ ", segments));
        }

        [TestCase]
        public void TestCutTraditionalChinese()
        {
            var seg = new JiebaSegmenter();
            TestCutThenPrint(seg, "小明最近在學習機器學習和自然語言處理");
        }

        [TestCase]
        public void TestUserDict()
        {
            var dict = @"Resources\user_dict.txt";
            var seg = new JiebaSegmenter();

            TestCutThenPrint(seg, "小明最近在学习机器学习、自然语言处理、云计算和大数据");
            seg.LoadUserDict(dict);
            TestCutThenPrint(seg, "小明最近在学习机器学习、自然语言处理、云计算和大数据");
        }

        [TestCase]
        public void TestSplit_Han_Default()
        {
            var s = "IBM是一家不错的公司，给你发offer了吗？";
            foreach (var part in JiebaSegmenter.RegexChineseDefault.Split(s))
            {
                Console.WriteLine(part);
            }

            foreach (var part in JiebaSegmenter.RegexChineseCutAll.Split(s))
            {
                Console.WriteLine(part);
            }
        }

        [TestCase]
        [Category("Issue")]
        public void TestEnglishWordsCut()
        {
            var seg = new JiebaSegmenter();
            var text = "HighestDegree";
            CollectionAssert.AreEqual(new[] { text }, seg.Cut(text));
            text = "HelloWorld";
            CollectionAssert.AreEqual(new[] { text }, seg.Cut(text));
            text = "HelloWorldle";
            CollectionAssert.AreEqual(new[] { text }, seg.Cut(text));
            text = "HelloWorldlee";
            CollectionAssert.AreEqual(new[] { text }, seg.Cut(text));
        }

        #region Private Helpers

        private void TestCutFunction(Func<string, bool, bool, IEnumerable<string>> method,
                                     bool cutAll, bool useHmm,
                                     string testResultFile)
        {
            var testCases = GetTestSentences();
            var testResults = File.ReadAllLines(testResultFile);
            Assert.That(testCases.Length, Is.EqualTo(testResults.Length));
            for (int i = 0; i < testCases.Length; i++)
            {
                var testCase = testCases[i];
                var testResult = testResults[i];
                Assert.That(method(testCase, cutAll, useHmm).Join("/ "), Is.EqualTo(testResult));
            }
        }

        private void TestCutSearchFunction(Func<string, bool, IEnumerable<string>> method,
                                     bool useHmm,
                                     string testResultFile)
        {
            var testCases = GetTestSentences();
            var testResults = File.ReadAllLines(testResultFile);
            Assert.That(testCases.Length, Is.EqualTo(testResults.Length));
            for (int i = 0; i < testCases.Length; i++)
            {
                var testCase = testCases[i];
                var testResult = testResults[i];
                Assert.That(method(testCase, useHmm).Join("/ "), Is.EqualTo(testResult));
            }
        }

        #endregion
    }
}