import React, { useContext } from "react";
import { AnswerContext } from "../../contexts/answer-context";

const AnswerOptionSelect = ({ question, answerOptions }) => {
  const context = useContext(AnswerContext);

  const onAnswerSelect = (e, option) => {
    context.saveAnswer(question, option);
  };

  const answer = context.answers.find(
    (z) => z.questionId === question.id
  )?.selectAnswer;

  return (
    <div className="answer-option">
      {answerOptions &&
        answerOptions.map((option) => {
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
