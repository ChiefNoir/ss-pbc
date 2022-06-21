import "./error-component.scss";

function ErrorComponent(props: {message: string, detail: string | null}) {
  return (
  <div className="container-error">
    <h2> {props.message}</h2>
    <h3> {props.detail}</h3>
  </div>
  );
};

export { ErrorComponent };
