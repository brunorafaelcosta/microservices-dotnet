import React, { Suspense } from 'react'
import PropTypes from 'prop-types'

import { ConfigContext } from 'web-common/src/context'
import { Bootstrap } from 'web-common/src/components'
import 'web-common/src/_services/_mock/index' // place this on the base api

import { baseConfig, baseRoutes } from './config'
import '_assets/scss/index.scss'

class App extends React.Component {
  constructor(props) {
    super(props)

    this.config = Object.assign(baseConfig, props.moduleConfig)
    
    this.moduleRoutes = props.moduleRoutes(this.config)
    this.baseRoutes = baseRoutes(this.config)
    this.routes = [ ...this.moduleRoutes, ...this.baseRoutes]

    this.state = {}
  }

  render() {
    return (
      <Suspense fallback={null}>
        <ConfigContext.Provider value={this.config}>
            <Bootstrap config={this.config} routes={this.routes}>
            </Bootstrap>
        </ConfigContext.Provider>
      </Suspense>
    );
  }
}
App.propTypes = {
  moduleConfig: PropTypes.object.isRequired,
  moduleRoutes: PropTypes.func.isRequired
}

export default App