export const persistanceLocalStorageMiddleware = (store) => (next) => (action) => {
  next(action)
  window.localStorage.setItem('__redux__state__', JSON.stringify(store.getState()))
}
