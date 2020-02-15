import React from 'react'

const AppContext = React.createContext(
  {
    authentication: {
      loggedIn: false,
      user: { }
    },
    localization: {},
    theme: {}
  })

export default AppContext