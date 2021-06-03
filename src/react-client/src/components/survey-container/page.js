import React from "react";
import Question from "./question";

const Page = ({surveyQuestions, paginationType}) => {
  
  return (
    <div className="page">
      {surveyQuestions && surveyQuestions.map((item) => <Question key={item.id} question={item} />)}
    </div>
  );
};

export default Page;
