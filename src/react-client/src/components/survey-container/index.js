import React, { useEffect, useState } from "react";
import api from "../../utils/api";
import queryString from "query-string";
import Header from "./header";
import Page from "./page";
import ActionBox from "./action-box";
import { PaginationType, APIURL, QuestionType } from "./../../utils/contants";
import PageLoader from "../elements/page-loader";
import Dialog from "../elements/dialog";
import { AnswerContext } from "../../contexts/answer-context";

const SurveyContainer = () => {
  const [loading, setLoading] = useState(false);
  const [dialog, setDialog] = useState({
    header: "",
    content: "",
    enable: false,
  });
  const [survey, setSurvey] = useState(null);
  const [pageInfo, setPageInfo] = useState({
    currentPage: 0,
    pageCount: 0,
  });
  const [preparedQuestions, setPreparedQuestions] = useState([]);
  const [answers, setAnswers] = useState([]);
  let currentPageQuestions = [];

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
          if (res.data.data) setSurvey(res.data.data);
          else setDialog({header: "", content: res.data.message, enable: true});
        } else {
          setDialog({header: "", content: res.data.message, enable: true});
        }
      })
      .catch((err) => {
        setDialog({header: "", content: err.response.data.message, enable: true});
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

  const onPageChangeHandle = (currentPage) => {
    if (pageInfo.currentPage < currentPage && !checkRequiredQuestions()) return;

    setPageInfo({ ...pageInfo, currentPage });
  };

  const checkRequiredQuestions = () => {
    const requiredQuestions = currentPageQuestions.filter(
      (z) => z.required === true
    );
    if (requiredQuestions.length === 0) return true;

    let requiredQuestionValidation = true;
    requiredQuestions.forEach((question) => {
      const answer = answers.find((z) => z.questionId === question.id);
      if (!answer) {
        requiredQuestionValidation = false;
        return;
      }

      if (answer.questionType === QuestionType.Text && !answer.textAnswer) {
        requiredQuestionValidation = false;
        return;
      }

      if (answer.questionType === QuestionType.Select && !answer.selectAnswer) {
        requiredQuestionValidation = false;
        return;
      }

      if (
        answer.questionType === QuestionType.MultiSelect &&
        (!answer.multiSelectAnswer || answer.multiSelectAnswer.length === 0)
      ) {
        requiredQuestionValidation = false;
        return;
      }
    });

    if(!requiredQuestionValidation){
      setDialog({header: "Error", content: "You should answer all required questions for next step.", enable: true});
    }

    return requiredQuestionValidation;
  };

  const onCompleteHandle = () => {
    if (!checkRequiredQuestions()) return;

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
          setDialog({header: "Success", content: "The survey have been completed successfully.", enable: true});

        } else {
          setDialog({header: "Error", content: res.data.message, enable: true});
        }
      })
      .catch((err) => {
        console.log(err);
        setDialog({header: "Error", content: err.response.data.message, enable: true});
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const renderOrderQuestion = () => {
    if (
      survey.paginationType !== PaginationType.Order ||
      !preparedQuestions ||
      preparedQuestions.length === 0
    )
      return null;

    const _questions = preparedQuestions.find(
      (z) => z.pageNumber === pageInfo.currentPage
    )?.questions;

    currentPageQuestions(_questions);

    return <Page questions={_questions} />;
  };

  const renderSurveyGroupQuestion = () => {
    if (
      survey.paginationType !== PaginationType.SurveyQuestionGroup ||
      !preparedQuestions ||
      preparedQuestions.length === 0
    )
      return null;

    const pageDefinition = preparedQuestions.find(
      (z) => z.pageNumber === pageInfo.currentPage
    );

    currentPageQuestions = pageDefinition?.questions;

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
    } else {
      const _answer = {
        questionId: question.id,
        questionType: question.questionType,
        additionalAnswer: text,
        textAnswer: null,
        selectAnswer: null,
        multiSelectAnswer: null,
      };
      setAnswers([...answers, _answer]);
    }
  };

  return (
    <>
      {loading && <PageLoader />}
      {dialog.enable && <Dialog header={dialog.header} content={dialog.content} onCloseHandler={() => setDialog({...dialog, enable: false})} />}
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
              onPageChangeHandle={onPageChangeHandle}
              onCompleteHandle={onCompleteHandle}
            />
          </div>
        </AnswerContext.Provider>
      )}
    </>
  );
};

export default SurveyContainer;
