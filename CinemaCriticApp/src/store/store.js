import { configureStore } from '@reduxjs/toolkit'
import commentReducer from '../comments/commentsSlice'
import { persistanceLocalStorageMiddleware } from '../middleware/persistanceLocalStorage'

export const store = configureStore({
  reducer: {
    comment: commentReducer
  },
  middleware: [persistanceLocalStorageMiddleware]
})
