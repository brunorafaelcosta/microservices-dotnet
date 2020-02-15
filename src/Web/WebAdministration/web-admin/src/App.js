import React from 'react'
import { default as BaseApp } from 'web-admin-common/src/App'

import { moduleConfig, moduleRoutes } from './config'
import '_assets/scss/index.scss'

class App extends React.Component {
  constructor() {
    super()
  }

  render() {
    return (
      <BaseApp 
        moduleConfig={moduleConfig} 
        moduleRoutes={moduleRoutes}
      />
    );
  }
}

export default App
