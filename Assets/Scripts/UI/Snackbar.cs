using System.Collections;
using UnityEngine;

public class Snackbar : SingletonMonoBehaviour<Snackbar>
{

    [SerializeField] private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        // Se estiver nulo (obj criado por código), tenta pegar o componente no mesmo GameObject
        if (_animator == null) _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (_animator == null)
        {
            Debug.LogWarning("Snackbar: Animator não atribuído. Desativando snackbar.", this);
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(PlayAnimationAndDisable());
    }

    public void ShowSnackbar()
    {
        gameObject.SetActive(true);
    }

    private IEnumerator PlayAnimationAndDisable()
    {
        float animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);
        gameObject.SetActive(false);
    }
}
