import React, { useContext } from "react";
import { AnswerContext } from "../../contexts/answer-context";

const AnswerOptionText = ({ question }) => {
  const context = useContext(AnswerContext);

  const onAnswerEnter = (e) => {
    context.saveAnswer(question, e.target.value);
  };

  const answer = context.answers.find(
    (z) => z.questionId === question.id
  )?.textAnswer;

  return (
    <div className="answer-option">
      <div className="answer-option-text">
        <textarea
          value={answer}
          rows="3"
          onChange={(e) => onAnswerEnter(e)}
        ></textarea>
      </div>
    </div>
  );
};

export default AnswerOptionText;
