import * as React from "react";
import AdditionalAnswer from "./additional-answer";
import AnswerOptionText from "./answer-option-text";
import AnswerOptionSelect from "./answer-option-select";
import AnswerOptionMultiSelect from "./answer-option-multi-select";

import { QuestionType } from "./../utils/constants";

const Question = ({ question }: any) => {
  return (
    <div className="question">
      <div className="question-content">
        <span>{question.order}. </span>
        {question.required && <span className="required-question"> * </span>}
        <span>{question.name}</span>
      </div>

      {question.questionType === QuestionType.Text && (
        <AnswerOptionText question={question} />
      )}
      {question.questionType === QuestionType.Select && (
        <AnswerOptionSelect
          question={question}
          answerOptions={question.answerOptions}
        />
      )}
      {question.questionType === QuestionType.MultiSelect && (
        <AnswerOptionMultiSelect
          question={question}
          answerOptions={question.answerOptions}
        />
      )}

      {question.additionalAnswer && <AdditionalAnswer question={question} />}
    </div>
  );
};

export default Question;
