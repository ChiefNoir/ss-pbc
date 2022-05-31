import "./project-component.scss";
import { ButtonGlitch } from "../../../../ui";
import { Format } from "../../../../helpers";
import { FunctionComponent } from "react";
import { Project } from "../../../../services";
import { useTranslation } from "react-i18next";
import { NavLink } from "react-router-dom";

const ProjectComponent: FunctionComponent<{project: Project}> = (props) => {
    const { t } = useTranslation();
    const project = props.project;

    return(
<div className="project-container">
    <h1>{project.displayName}</h1>

    <div className="project-container-content">
        <div className="project-container-content-meta">
            <img className="project-container-content-poster" 
                alt={(project.posterDescription ?? project.displayName)}
                src={(project.posterUrl ?? "/assets/images/placeholder-wide.png")}/>
            {
            project.externalUrls?.map (
                x => { 
                    return <ButtonGlitch key={x.id} displayName={x.displayName} url= {x.url} />
                     })
            }
        </div>

        <div className="project-container-content-description">
            <b>{t("Project.Category")}: </b>
            <NavLink to={`/projects?category=${project.category.code}`}>
                {project.category.displayName}
            </NavLink>
            <br/>
            <b>{t("Project.ReleaseDate")}: </b>{Format.ToDate(project.releaseDate)}
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
