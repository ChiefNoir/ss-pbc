import "./button-glitch.scss";

function ButtonGlitch(props: {displayName: string, url:string}) {
  return (
    <a className="glitch-button"
       rel="noreferrer noopener"
       target="_blank"
       href={props.url}
       data-text={props.displayName}>
         {props.displayName}
    </a>
  );
};

export { ButtonGlitch };
