import "./buttonContact.scss";

function ButtonContact(key: any, url: string, displayName: string) {
  return (
    <a className="button-contact"
       rel="noreferrer noopener"
       target="_blank"
       key = { key } 
       href = { url }>
      <span> { displayName } </span>
    </a>
)};

export default ButtonContact;
