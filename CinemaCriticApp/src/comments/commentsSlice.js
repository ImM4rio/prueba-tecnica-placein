import { createSlice } from '@reduxjs/toolkit'
import { v4 as uuid } from 'uuid'

const DEFAULT_STATE = [
  {
    key: 1,
    title: 'Bailando con Mario',
    rating: 9,
    comment: 'Es una película excelente, Mario baila muy bien'
  },
  {
    key: 2,
    title: 'Juegos del hambre: Mario en llamas',
    rating: 1,
    comment: 'No me gustó, el vestido que llevaba Mario cuando iba en la carroza no era muy bonito'
  }
]

const initialState = () => {
  const persistedState = window.localStorage.getItem('__redux__state__')
  if (persistedState) {
    return JSON.parse(persistedState).comment
  }
  return DEFAULT_STATE
}

export const commentsSlice = createSlice({
  name: 'comment',
  initialState,
  reducers: {
    deleteComment: (state, action) => {
      const key = action.payload
      return state.filter((com) => com.key !== key)
    },
    addComment: (state, action) => {
      const key = uuid()
      return [...state, { key, ...action.payload }]
    }
  }
})

export const { deleteComment, addComment } = commentsSlice.actions
export default commentsSlice.reducer
