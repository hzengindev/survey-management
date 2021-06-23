import React, { useEffect, useState } from "react";
import api from "../../utils/api";
import queryString from "query-string";
import Header from "./header";
import Page from "./page";
import ActionBox from "./action-box";
import { PaginationType, APIURL, QuestionType } from "./../../utils/contants";
import PageLoader from "../elements/page-loader";

import { AnswerContext } from "../../contexts/answer-context";

const SurveyContainer = () => {
  const [loading, setLoading] = useState(false);
  const [survey, setSurvey] = useState(null);
  const [pageInfo, setPageInfo] = useState({
    currentPage: 0,
    pageCount: 0,
  });
  const [preparedQuestions, setPreparedQuestions] = useState([]);
  const [answers, setAnswers] = useState([]);

  useEffect(() => {
    var query = queryString.parse(window.location.search);
    if (!query?.code) {
      return;
    }

    setLoading(true);
    api
      .get(`/survey/${query.code}`)
      .then((res) => {
        if (res.data.success) {
          setSurvey(res.data.data);
        } else {
          console.log(res.data.message);
        }
      })
      .catch((err) => {
        console.log(err);
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  useEffect(() => {
    if (survey) prepareQuestions();
  }, [survey]);

  const prepareQuestions = () => {
    if (survey.paginationType === PaginationType.Order)
      prepareOrderedQuestion();

    if (survey.paginationType === PaginationType.SurveyQuestionGroup)
      prepareGroupedQuestion();
  };

  const prepareOrderedQuestion = () => {
    const _preparedQuestions = [];
    const pageCount = Math.ceil(
      survey.surveyQuestions.length / survey.recordPerPage
    );

    for (let i = 0; i < pageCount; i++) {
      const start = (i + 1) * survey.recordPerPage - survey.recordPerPage;
      const end = (i + 1) * survey.recordPerPage;

      const q = {
        pageNumber: i,
        groupName: "",
        groupDescription: "",
        showGroupDescription: false,
        questions: survey.surveyQuestions.slice(start, end),
      };

      _preparedQuestions.push(q);
    }
    setPreparedQuestions(_preparedQuestions);
    setPageInfo({ ...pageInfo, pageCount });
  };

  const prepareGroupedQuestion = () => {
    const _preparedQuestions = [];

    let groups = [];
    survey.surveyQuestions.forEach((question) => {
      if (
        question.surveyQuestionGroup &&
        !groups.find((z) => z.id === question.surveyQuestionGroup.id)
      )
        groups.push(question.surveyQuestionGroup);
    });

    groups = groups.sort((a, b) => a.order - b.order);

    for (let i = 0; i < groups.length; i++) {
      const q = {
        pageNumber: i,
        groupName: groups[i].name,
        groupDescription: groups[i].description,
        showGroupDescription: groups[i].showDescription,
        questions: survey.surveyQuestions.filter(
          (z) => z?.surveyQuestionGroup.id === groups[i].id
        ),
      };
      _preparedQuestions.push(q);
    }

    setPreparedQuestions(_preparedQuestions);
    setPageInfo({ ...pageInfo, pageCount: groups.length });
  };

  const onPageHandle = (currentPage) => {
    setPageInfo({ ...pageInfo, currentPage });
  };

  const onCompleteHandle = () => {
    var query = queryString.parse(window.location.search);
    if (!query?.code) {
      return;
    }

    const payload = {
      code: query?.code,
      answers: answers,
    };

    setLoading(true);
    api
      .post(`/survey/complete`, payload)
      .then((res) => {
        if (res.data.success) {
        } else {
          console.log(res.data.message);
        }
      })
      .catch((err) => {
        console.log(err);
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const renderOrderQuestion = () => {
    if (survey.paginationType !== PaginationType.Order || !preparedQuestions)
      return null;

    const _questions = preparedQuestions.find(
      (z) => z.pageNumber === pageInfo.currentPage
    )?.questions;

    return <Page questions={_questions} />;
  };

  const renderSurveyGroupQuestion = () => {
    if (
      survey.paginationType !== PaginationType.SurveyQuestionGroup ||
      !preparedQuestions
    )
      return null;

    const pageDefinition = preparedQuestions.find(
      (z) => z.pageNumber === pageInfo.currentPage
    );

    return (
      <div className="survey-question-group">
        <h2 className="survey-question-group-header">
          {pageDefinition?.groupName}
        </h2>
        {pageDefinition?.showGroupDescription && (
          <p className="survey-question-group-description">
            {pageDefinition?.groupDescription}
          </p>
        )}
        <Page questions={pageDefinition?.questions} />
      </div>
    );
  };

  const saveAnswer = (question, answer) => {
    const _answer = answers.find((z) => z.questionId === question.id);
    if (_answer) {
      _answer.textAnswer =
        question.questionType === QuestionType.Text ? answer : null;
      _answer.selectAnswer =
        question.questionType === QuestionType.Select ? answer.id : null;
      _answer.multiSelectAnswer =
        question.questionType === QuestionType.MultiSelect
          ? answer.map((a) => a.id)
          : null;
      setAnswers([
        ...answers.filter((z) => z.questionId !== question.id),
        _answer,
      ]);
    } else {
      const _answer = {
        questionId: question.id,
        questionType: question.questionType,
        additionalAnswer: "",
        textAnswer: question.questionType === QuestionType.Text ? answer : null,
        selectAnswer:
          question.questionType === QuestionType.Select ? answer.id : null,
        multiSelectAnswer:
          question.questionType === QuestionType.MultiSelect
            ? answer.map((a) => a.id)
            : null,
      };
      setAnswers([...answers, _answer]);
    }
  };

  const saveAdditionalAnswer = (question, text) => {
    const _answer = answers.find((z) => z.questionId === question.id);
    if (_answer) {
      _answer.additionalAnswer = text;
      setAnswers([
        ...answers.filter((z) => z.questionId !== question.id),
        _answer,
      ]);
    }
  };

  return (
    <>
      {loading && <PageLoader />}
      {!survey && (
        <div className="home">
          <h1>Survey Management</h1>
        </div>
      )}
      {survey && (
        <AnswerContext.Provider
          value={{ answers, saveAnswer, saveAdditionalAnswer }}
        >
          <div className="survey-container">
            <Header
              name={survey.name}
              description={survey.description}
              imageURL={APIURL + survey.imageURL}
            />
            {renderOrderQuestion()}
            {renderSurveyGroupQuestion()}
            <ActionBox
              pageInfo={pageInfo}
              onPageHandle={onPageHandle}
              onCompleteHandle={onCompleteHandle}
            />
          </div>
        </AnswerContext.Provider>
      )}
    </>
  );
};

export default SurveyContainer;
