import { Routes, Route } from 'react-router-dom'
import React, { lazy, Suspense } from 'react'
import './stylesheets/App.css'

const Search = lazy(() => import('./pages/Search'))
const MyList = lazy(() => import('./pages/MyList'))

const App = () => {
  return (
    <>
      <Routes>
        <Route path='/' element={<Search />} />
        <Route path='/search' element={<Search />} />
        <Route path='/mylist' element={<MyList />} />
      </Routes>
    </>
  )
}

export const LazyApp = () => {
  return (
    <Suspense fallback={<div>Cargando...</div>}>
      <App />
    </Suspense>
  )
}

export default App
