import * as React from "react";
import { useEffect, useState } from "react";
import { AnswerContext } from "../contexts/answer-context";
import Dialog from "../elements/dialog";
import PageLoader from "../elements/page-loader";
import { Environment, PaginationType, QuestionType } from "../utils/constants";

import { mockData } from "../utils/mock-data";
import ActionBox from "./action-box";
import Header from "./header";
import Page from "./page";

export interface ISurveyContainerProps {
  surveyRequestId: string;
}

const SurveyContainer = (props: ISurveyContainerProps): JSX.Element => {
  const [loading, setLoading] = useState(false);
  const [dialog, setDialog] = useState({
    header: "",
    content: "",
    enable: false,
  });
  const [survey, setSurvey] = useState<any>(null);
  const [pageInfo, setPageInfo] = useState({
    currentPage: 0,
    pageCount: 0,
  });
  const [preparedQuestions, setPreparedQuestions] = useState<any>([]);
  const [answers, setAnswers] = useState([]);
  let currentPageQuestions: any | null = [];

  useEffect(() => {
    if (!props.surveyRequestId) {
      setDialog({ header: "", content: "There is no survey request.", enable: true });
      return;
    }
    debugger;
    if (Environment.IsDev) {
      setLoading(true);
      setSurvey(mockData.GetSurvey);
      setLoading(false);
    } else {
      let _request = {
        InputParameter: JSON.stringify({
          surveyRequestId: props.surveyRequestId,
        }),
        getMetadata: function () {
          return {
            boundParameter: null,
            parameterTypes: {
              InputParameter: {
                typeName: "Edm.String",
                structuralProperty: 1,
              },
            },
            operationType: 0,
            operationName: "hz_GetSurvey",
          };
        },
      };

      setLoading(true);
      Xrm.WebApi.online
        .execute(_request)
        .then((response) => response.json())
        .then((json: any) => {
          debugger;
          console.log(json);

          const actionResult = JSON.parse(json.ActionResult);

          if (actionResult.success) {
            const dataResult = JSON.parse(json.DataResult);
            setSurvey(dataResult);
          } else {
            setDialog({
              header: "",
              content: actionResult.message,
              enable: true,
            });
          }
        })
        .catch((err) => {
          debugger;
          console.log(err);
          setDialog({ header: "", content: err.message, enable: true });
        })
        .finally(() => {
          setLoading(false);
        });
    }
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
    const _preparedQuestions: any = [];
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
    const _preparedQuestions: any = [];

    let groups: any = [];
    survey.surveyQuestions.forEach((question: any) => {
      if (
        question.surveyQuestionGroup &&
        !groups.find((z: any) => z.id === question.surveyQuestionGroup.id)
      )
        groups.push(question.surveyQuestionGroup);
    });

    groups = groups.sort((a: any, b: any) => a.order - b.order);

    for (let i = 0; i < groups.length; i++) {
      const q = {
        pageNumber: i,
        groupName: groups[i].name,
        groupDescription: groups[i].description,
        showGroupDescription: groups[i].showDescription,
        questions: survey.surveyQuestions.filter(
          (z: any) => z?.surveyQuestionGroup.id === groups[i].id
        ),
      };
      _preparedQuestions.push(q);
    }

    setPreparedQuestions(_preparedQuestions);
    setPageInfo({ ...pageInfo, pageCount: groups.length });
  };

  const onPageChangeHandle = (currentPage: number) => {
    if (pageInfo.currentPage < currentPage && !checkRequiredQuestions()) return;

    setPageInfo({ ...pageInfo, currentPage });
  };

  const checkRequiredQuestions = () => {
    const requiredQuestions = currentPageQuestions.filter(
      (z: any) => z.required === true
    );
    if (requiredQuestions.length === 0) return true;

    let requiredQuestionValidation = true;
    requiredQuestions.forEach((question: any) => {
      const answer: any = answers.find(
        (z: any) => z.questionId === question.id
      );
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

    if (!requiredQuestionValidation) {
      setDialog({
        header: "Error",
        content: "You should answer all required questions for next step.",
        enable: true,
      });
    }

    return requiredQuestionValidation;
  };

  const onCompleteHandle = () => {
    if (!checkRequiredQuestions()) return;

    if (!props.surveyRequestId) {
      return;
    }

    const payload = {
      surveyRequestId: props.surveyRequestId,
      answers: answers,
    };

    let _request = {
      InputParameter: JSON.stringify(payload),
      getMetadata: function () {
        return {
          boundParameter: null,
          parameterTypes: {
            InputParameter: {
              typeName: "Edm.String",
              structuralProperty: 1,
            },
          },
          operationType: 0,
          operationName: "hz_CompleteSurvey",
        };
      },
    };

    setLoading(true);
    Xrm.WebApi.online
      .execute(_request)
      .then((response) => response.json())
      .then((json: any) => {
        debugger;
        console.log(json);

        const actionResult = JSON.parse(json.ActionResult);

        if (actionResult.success) {
          setDialog({header: "Success", content: "The survey have been completed successfully.", enable: true});
        } else {
          setDialog({
            header: "",
            content: actionResult.message,
            enable: true,
          });
        }
      })
      .catch((err) => {
        debugger;
        console.log(err);
        setDialog({ header: "", content: err.message, enable: true });
      })
      .finally(() => {
        setLoading(false);
      });

    

    // setLoading(true);
    // api
    //   .post(`/survey/complete`, payload)
    //   .then((res) => {
    //     if (res.data.success) {
    //       

    //     } else {
    //       setDialog({header: "Error", content: res.data.message, enable: true});
    //     }
    //   })
    //   .catch((err) => {
    //     console.log(err);
    //     setDialog({header: "Error", content: err.response.data.message, enable: true});
    //   })
    //   .finally(() => {
    //     setLoading(false);
    //   });
  };

  const renderOrderQuestion = () => {
    if (
      survey.paginationType !== PaginationType.Order ||
      !preparedQuestions ||
      preparedQuestions.length === 0
    )
      return null;

    const _questions: any = preparedQuestions.find(
      (z: any) => z.pageNumber === pageInfo.currentPage
    )?.questions;

    currentPageQuestions = _questions;

    return <Page questions={_questions} />;
  };

  const renderSurveyGroupQuestion = () => {
    if (
      survey.paginationType !== PaginationType.SurveyQuestionGroup ||
      !preparedQuestions ||
      preparedQuestions.length === 0
    )
      return null;

    const pageDefinition: any = preparedQuestions.find(
      (z: any) => z.pageNumber === pageInfo.currentPage
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

  const saveAnswer = (question: any, answer: any) => {
    const _answer: any = answers.find((z: any) => z.questionId === question.id);
    if (_answer) {
      _answer.textAnswer =
        question.questionType === QuestionType.Text ? answer : null;
      _answer.selectAnswer =
        question.questionType === QuestionType.Select ? answer.id : null;
      _answer.multiSelectAnswer =
        question.questionType === QuestionType.MultiSelect
          ? answer.map((a: any) => a.id)
          : null;

      const _answers: any = [
        ...answers.filter((z: any) => z.questionId !== question.id),
        _answer,
      ];

      setAnswers(_answers);
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
            ? answer.map((a: any) => a.id)
            : null,
      };
      const _answers: any = [...answers, _answer];
      setAnswers(_answers);
    }
  };

  const saveAdditionalAnswer = (question: any, text: any) => {
    const _answer: any = answers.find((z: any) => z.questionId === question.id);
    if (_answer) {
      _answer.additionalAnswer = text;
      const _answers: any = [
        ...answers.filter((z: any) => z.questionId !== question.id),
        _answer,
      ];
      setAnswers(_answers);
    } else {
      const _answer = {
        questionId: question.id,
        questionType: question.questionType,
        additionalAnswer: text,
        textAnswer: null,
        selectAnswer: null,
        multiSelectAnswer: null,
      };
      const _answers: any = [...answers, _answer];
      setAnswers(_answers);
    }
  };

  return (
    <>
      {loading && <PageLoader />}
      {dialog.enable && (
        <Dialog
          header={dialog.header}
          content={dialog.content}
          onCloseHandler={() => setDialog({ ...dialog, enable: false })}
        />
      )}
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
              imageURL={survey.imageURL}
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
