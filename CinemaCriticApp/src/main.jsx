import { BrowserRouter } from 'react-router-dom'
import ReactDOM from 'react-dom/client'
import LazyApp from './App'
import './stylesheets/index.css'
import { store } from './store/store'
import { Provider } from 'react-redux'

ReactDOM.createRoot(document.getElementById('root')).render(
  <Provider store={store}>
    <BrowserRouter>
      <LazyApp />
    </BrowserRouter>
  </Provider>
)