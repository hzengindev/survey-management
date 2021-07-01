import * as React from "react";
import { useContext, useState } from "react";
import { AnswerContext } from "./../contexts/answer-context";

const AnswerOptionSelect = ({ question, answerOptions }: any): JSX.Element => {
  const context: any = useContext(AnswerContext);

  const onAnswerSelect = (
    e: React.ChangeEvent<HTMLInputElement>,
    option: any
  ) => {
    context.saveAnswer(question, option);
  };

  const answer = context.answers.find(
    (z: any) => z.questionId === question.id
  )?.selectAnswer;

  return (
    <div className="answer-option">
      {answerOptions &&
        answerOptions.map((option: any) => {
          return (
            <div className="answer-option-select" key={option.id}>
              <label>
                <input
                  type="radio"
                  name={`question-${question.order}`}
                  value={option.id}
                  checked={answer === option.id}
                  onChange={(e) => onAnswerSelect(e, option)}
                />
                {option.name}
              </label>
            </div>
          );
        })}
    </div>
  );
};

export default AnswerOptionSelect;
