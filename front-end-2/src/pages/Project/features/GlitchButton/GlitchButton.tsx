import { FunctionComponent } from 'react';
import './GlitchButton.scss';

const GlitchButton: FunctionComponent<{displayName: string, url:string}> = (props) => {
    return(
        <a className="glitch-button"
           rel="noreferrer noopener"
           target="_blank"
           href={props.url}
           data-text={props.displayName}>
               {props.displayName}
        </a>
    )
}

export { GlitchButton };
