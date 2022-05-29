import { FunctionComponent } from "react";
import "./button-glitch.scss";

const ButtonGlitch: FunctionComponent<{displayName: string, url:string}> = (props) => {
    return(
        <a className="glitch-button"
           rel="noreferrer noopener"
           target="_blank"
           href={ props.url }
           data-text={ props.displayName }>
               {props.displayName}
        </a>
    )
}

export { ButtonGlitch };
