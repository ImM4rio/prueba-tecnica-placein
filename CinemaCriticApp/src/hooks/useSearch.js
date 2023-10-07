import { useState, useEffect, useRef } from 'react'

export const useSearch = () => {
  const [error, setError] = useState(null)
  const [query, setQuery] = useState('')
  const usedInput = useRef(true)

  useEffect(() => {
    if (usedInput.current) {
      usedInput.current = query === ''
      return
    }

    if (query === '') {
      setError('No se encontró ningún resultado para esa búsqueda')
      return
    }

    if (query.length < 3) {
      setError('La búsqueda debe tener al menos 3 caracteres')
      return
    }
    setError(null)
  }, [query])

  return { query, setQuery, error }
}
