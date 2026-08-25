
export const PrimaryButton = ({ btnText, onClick }) => {
  return (
        <>
            <button type="button" className="btn btn-primary" onClick={onClick}>{btnText || ""}</button>
        </>
  )
}
