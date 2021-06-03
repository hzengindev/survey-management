import React, {useState} from "react";
import Header from "./header";
import Page from "./page";
import ActionBox from "./action-box";
import { PaginationType } from "./../../utils/contants";
import {SurveyData} from "./../../utils/mock-data";

const SurveyContainer = () => {
  
  const [survey, setSurvey] = useState(SurveyData);

  return (
    <div className="survey-container">
      <Header
        name={survey.name}
        description={survey.description}
        imageURL={survey.imageURL}
      />

      {survey.paginationType === PaginationType.Order && (
        <Page surveyQuestions={survey.surveyQuestions} />
      )}

      {survey.paginationType === PaginationType.SurveyQuestionGroup && (
        <div className="survey-question-group">
          <h2 className="survey-question-group-header">Group Name</h2>
          <Page surveyQuestions={survey.surveyQuestions} />
        </div>
      )}

      <ActionBox />
    </div>
  );
};

export default SurveyContainer;
