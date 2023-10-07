import '../stylesheets/App.css'
import { Movies } from '../components/Movies'
import { useMovies } from '../hooks/useMovies'
import { useSearch } from '../hooks/useSearch'

function Search () {
  const { query, setQuery, error } = useSearch()
  const { movies, getMovies } = useMovies({ query })

  const handleSubmit = (event) => {
    event.preventDefault()
    getMovies()
  }

  const handleChange = (event) => {
    const newQuery = event.target.value
    setQuery(newQuery)
  }

  return (
    <div className='search'>
      <nav>
        <a href='/'>Home</a>
        <a href='/mylist'>Vamonos a mi lista</a>
      </nav>
      <header>
        <h1>Listado de películas</h1>
        <form className='form' onSubmit={handleSubmit}>
          <input onChange={handleChange} name='inputTxt' value={query} placeholder='Busca tu película...' />
          <button type='submit'>Buscar</button>
        </form>
        {error && <p>{error}</p>}
      </header>
      <main>
        <Movies movies={movies} />
      </main>
    </div>
  )
}

export default Search
