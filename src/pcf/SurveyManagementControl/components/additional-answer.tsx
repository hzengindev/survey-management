import * as React from "react";
import { useContext } from "react";
import { AnswerContext } from "./../contexts/answer-context";

const AdditionalAnswer = ({ question }: any): JSX.Element => {
  const context: any = useContext(AnswerContext);

  const onAnswerEnter = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    context.saveAdditionalAnswer(question, e.target.value);
  };

  const additionalAnswer = context.answers.find(
    (z: any) => z.questionId === question.id
  )?.additionalAnswer;

  return (
    <div className="additional-answer">
      <span>Please, enter your additional answer.</span>
      <textarea
        rows={3}
        defaultValue={additionalAnswer}
        onChange={(e) => onAnswerEnter(e)}
      ></textarea>
    </div>
  );
};

export default AdditionalAnswer;
