import * as React from "react";
import Question from "./question";

const Page = ({ questions }: any): JSX.Element => {
  return (
    <div className="page">
      {questions &&
        questions.map((item: any) => (
          <Question key={item.id} question={item} />
        ))}
    </div>
  );
};

export default Page;
