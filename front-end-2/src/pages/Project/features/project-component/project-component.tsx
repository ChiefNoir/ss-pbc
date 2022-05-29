import "./project-component.scss";
import { ButtonGlitch } from "../../../../ui";
import { Format } from "../../../../helpers";
import { FunctionComponent } from "react";
import { Project } from "../../../../services";
import { useTranslation } from "react-i18next";

const ProjectComponent: FunctionComponent<{project: Project}> = (props) => {
    const { t } = useTranslation();
    const project = props.project;

    return(
<div className="container-project-main">
    <h1>{project.displayName}</h1>

    <div className="project-data">
        <div className="project-data-left-panel">
            <img className="poster" 
                alt={(project.posterDescription ?? project.displayName)}
                src={(project.posterUrl ?? "/assets/images/placeholder-wide.png")}/>
            {
            project.externalUrls?.map (
                x => { 
                    return <ButtonGlitch key={x.id} displayName={x.displayName} url= {x.url} />
                     })
            }
        </div>

        <div className="project-data-right-panel">
            <div><b>{t("Project.Category")}: </b>{project.category.displayName}</div>
            <div><b>{t("Project.ReleaseDate")}: </b>{Format.ToDate(project.releaseDate)}</div>
            <hr/>

            <div className="project-description"
                 dangerouslySetInnerHTML={{__html: project.description ?? ""}}>
            </div>
        </div>
    </div>
</div>
    )
}

export { ProjectComponent }; 
