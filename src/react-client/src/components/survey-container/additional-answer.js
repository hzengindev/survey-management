import React, { useContext } from "react";
import { AnswerContext } from "../../contexts/answer-context";

const AdditionalAnswer = ({question}) => {
  const context = useContext(AnswerContext);

  const onAnswerEnter = (e) => {
    context.saveAdditionalAnswer(question, e.target.value);
  };

  const additionalAnswer = context.answers.find(
    (z) => z.questionId === question.id
  )?.additionalAnswer;

  return (
    <div className="additional-answer">
      <span>Please, enter your additional answer.</span>
      <textarea rows="3" defaultValue={additionalAnswer} onChange={(e) => onAnswerEnter(e)}></textarea>
    </div>
  );
};

export default AdditionalAnswer;