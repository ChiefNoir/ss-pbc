# Front-end
This is the front-end and the main entry point of the project.

Build on a [React](https://reactjs.org/) with a [TypeScript](https://www.typescriptlang.org/) stack.

#features
General reusable non-business-related features of the website.
Like footer, navigation, buttons, etc.

#helpers
Reusable helpers and utility functions/classes.

# locales
i18n.
The translations are located at /public/locales/%lng%/translation.json
Even though the site does not have alternative languages, all static text must go there.

# pages
The core idea of ​​the project structure is: from page to feature.
So, every /pages/%feature-page% represents a clear feature of the project.
Inside the %feature-page% can be additional features/components linked to the scope of the parent one.

# pages-admin
Same as above, but for the admin panel.

#storage
[Redux](https://react-redux.js.org/) and localStorage interactions.

#ui
Generic UI elements.
Probably will soon go to the top-level features folder.

#Requirements
- [Microsoft Visual Studio Code](https://code.visualstudio.com/)
- [Node.js v18.0.0](https://nodejs.org/en/)
- Running back-end from the back-end project.