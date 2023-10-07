import { Comments } from '../components/Comments'

const MyList = () => {
  return (
    <div className='my-list'>
      <nav>
        <a href='/'>Home</a>
        <a href='/mylist'>Vamonos a mi lista</a>
      </nav>
      <header>
        <h1>Listado de películas con crítica</h1>
      </header>
      <main>
        <Comments />
      </main>
    </div>
  )
}

export default MyList
